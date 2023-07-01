using System.Text.RegularExpressions;
using ClosedXML.Excel;
using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Interfaces;
using ESM.Domain.Dtos.Examination;
using ESM.Domain.Entities;
using ESM.Domain.Enums;
using ESM.Domain.Interfaces;

namespace ESM.Presentation.Services;

public class ExaminationService : IExaminationService
{
    #region Properties

    private readonly IApplicationDbContext _context;

    private const int MethodColumn = 7;
    private const int DateColumn = 8;
    private const int ShiftColumn = 9;
    private const int DepartmentAssign = 15;

    private static readonly Dictionary<int, string> ExaminationDataMapping = new()
    {
        { 2, "ModuleId" },
        { 3, "ModuleName" },
        { 4, "ModuleClass" },
        { 12, "Rooms" },
        { 13, "Department" },
        { 14, "Faculty" }
    };

    private static readonly Dictionary<int, string> ExaminationDataIntMapping = new()
    {
        { 6, "Credit" },
        { 10, "CandidatesCount" },
        { 11, "RoomsCount" }
    };

    private static readonly int[] ExaminationDataHandleFields =
        { MethodColumn, DateColumn, ShiftColumn, DepartmentAssign };

    public ExaminationService(IApplicationDbContext context)
    {
        _context = context;
    }

    #endregion

    /// <summary>
    /// Import data
    /// </summary>
    /// <param name="file"></param>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    public List<ExaminationData> Import(IFormFile file, string examinationId)
    {
        using var wb = new XLWorkbook(file.OpenReadStream());

        var ws = GetWorkSheet(wb);
        var examinationsList = new List<ExaminationData>();

        GetColsAndRowsCount(ws, out var rowsCount, out var colsCount);

        for (var r = 2; r <= rowsCount; r++)
        {
            var row = ws.Row(r);
            var examinationData = new ExaminationData
            {
                ExaminationId = new Guid(examinationId)
            };

            for (var c = 1; c <= colsCount; c++)
            {
                if (TrySetPrimitiveTypesField(row, c, examinationData))
                    continue;

                TrySetCustomField(row, c, examinationData);
            }

            examinationsList.Add(examinationData);
        }

        return examinationsList;
    }

    public Guid CheckIfExaminationExistAndReturnGuid(string examinationId, ExaminationStatus? acceptStatus = null)
    {
        var guid = ParseGuid(examinationId);

        var status = _context.Examinations
            .Select(e => new { e.Id, e.Status })
            .FirstOrDefault(u => u.Id == guid)
            ?.Status;

        if (status == null)
        {
            throw new NotFoundException(nameof(Examination), guid);
        }

        if (acceptStatus != null && (acceptStatus.Value & status) == 0)
        {
            throw new BadRequestException($"Examination status should be {acceptStatus.ToString()}");
        }

        return guid;
    }

    public Examination CheckIfExaminationExistAndReturnEntity(string examinationId,
        ExaminationStatus? acceptStatus = null)
    {
        var guid = ParseGuid(examinationId);
        var entity = _context.Examinations.FirstOrDefault(e => e.Id == guid);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Examination), guid);
        }

        if (acceptStatus != null && (acceptStatus.Value & entity.Status) == 0)
        {
            throw new BadRequestException($"Examination status should be {acceptStatus.ToString()}");
        }

        return entity;
    }

    public void CalculateInvigilatorsNumberInShift<T>(T group, ICollection<FacultyShiftGroup> facultyShiftGroup,
        IReadOnlyDictionary<Guid, int> invigilatorsNumberInFaculties) where T : IShiftGroup
    {
        group.AssignNumerate = facultyShiftGroup
            .ToDictionary(
                fg => fg.FacultyId.ToString(),
                fg => new ShiftGroupDataCell
                {
                    Actual = fg.InvigilatorsCount,
                    Calculated = fg.CalculatedInvigilatorsCount,
                    Maximum = invigilatorsNumberInFaculties.GetValueOrDefault(fg.FacultyId, 0)
                }
            );

        var total = facultyShiftGroup.Sum(feg => feg.InvigilatorsCount);
        group.AssignNumerate.Add("total",
            new ShiftGroupDataCell
            {
                // Actual calculation result
                Actual = total,
                // Difference
                Calculated = total - group.InvigilatorsCount
            });
    }

    #region Private methods

    private static IXLWorksheet GetWorkSheet(IXLWorkbook wb)
    {
        var ws = wb.Worksheets.FirstOrDefault();
        if (ws == null)
            throw new BadRequestException("Worksheet is empty!");

        return ws;
    }

    private static void GetColsAndRowsCount(IXLWorksheet ws, out int rowsCount, out int colsCount)
    {
        rowsCount = 0;
        colsCount = 0;
        while (!ws.Cell(rowsCount + 1, 1).Value.IsBlank) rowsCount++;
        while (!ws.Cell(1, colsCount + 1).Value.IsBlank) colsCount++;
    }

    private static bool TrySetPrimitiveTypesField(IXLRow row, int columnIndex, ExaminationData examinationData)
    {
        return TrySetStringField(row, columnIndex, examinationData) ||
               TrySetIntField(row, columnIndex, examinationData);
    }

    private static void TrySetCustomField(IXLRow row, int columnIndex, ExaminationData examinationData)
    {
        if (!ExaminationDataHandleFields.Contains(columnIndex) || row.Cell(columnIndex).IsEmpty())
            return;

        var cellValue = row.Cell(columnIndex).Value;

        switch (columnIndex)
        {
            case DateColumn:
                examinationData.Date = cellValue.GetDateTime();
                break;

            case ShiftColumn:
                var cv = cellValue.GetText().ToLower();
                var timeStr = Regex.Matches(cv, @"\d\d", RegexOptions.None, TimeSpan.FromSeconds(1))
                    .Select(x => int.Parse(x.Value))
                    .ToArray();

                if (cv.Contains("ca"))
                {
                    var splitArr = cv.Split(" ");
                    examinationData.Shift = int.Parse(splitArr[1]);
                }

                examinationData.StartAt = examinationData.EndAt = examinationData.Date;

                examinationData.StartAt =
                    examinationData.StartAt?.Add(new TimeSpan(timeStr[0], timeStr[1], 0));
                examinationData.EndAt =
                    examinationData.EndAt?.Add(new TimeSpan(timeStr[2], timeStr[3], 0));

                break;

            case MethodColumn:
                examinationData.Method = ExamMethodHelper.FromString(cellValue.GetText().ToLower());
                break;

            case DepartmentAssign:
                examinationData.DepartmentAssign = cellValue.GetText().ToLower().Equals("bộ môn");
                break;
        }
    }

    private static bool TrySetStringField(IXLRow row, int columnIndex, ExaminationData examinationData)
    {
        if (!ExaminationDataMapping.TryGetValue(columnIndex, out var field))
            return false;

        if (row.Cell(columnIndex).IsEmpty())
            return true;

        var cellValue = row.Cell(columnIndex).GetText().Trim();
        typeof(ExaminationData).GetProperty(field)?.SetValue(examinationData, cellValue);
        return true;
    }

    private static bool TrySetIntField(IXLRow row, int columnIndex, ExaminationData examinationData)
    {
        if (!ExaminationDataIntMapping.TryGetValue(columnIndex, out var field))
            return false;

        if (row.Cell(columnIndex).IsEmpty())
            return true;

        var cellValue = Convert.ToInt32(row.Cell(columnIndex).GetDouble());
        typeof(ExaminationData).GetProperty(field)?.SetValue(examinationData, cellValue);
        return true;
    }

    private static Guid ParseGuid(string id)
    {
        if (!Guid.TryParse(id, out var guid))
        {
            throw new NotFoundException(nameof(Examination), id);
        }

        return guid;
    }

    #endregion
}