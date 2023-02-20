using ClosedXML.Excel;
using ESM.Common.Core.Exceptions;
using ESM.Data.Models;
using System.Text.RegularExpressions;
using ESM.API.Repositories.Implementations;
using ESM.Data.Enums;

namespace ESM.API.Services;

public class ExaminationService
{
    #region Properties

    private const int METHOD_COLUMN = 7;
    private const int DATE_COLUMN = 8;
    private const int SHIFT_COLUMN = 9;
    private const int DEPARTMENT_ASSIGN = 15;
    private readonly ModuleRepository _moduleRepository;

    private static readonly Dictionary<int, string> ExaminationDataMapping = new()
    {
        { 2, "ModuleId" },
        { 3, "ModuleName" },
        { 4, "ModuleClass" },
        { 12, "Rooms" },
        { 13, "Department" },
        { 14, "Faculty" },
    };

    private static readonly Dictionary<int, string> ExaminationDataIntMapping = new()
    {
        { 6, "Credit" },
        { 10, "CandidateCount" },
        { 11, "RoomsCount" }
    };

    private static readonly int[] ExaminationDataHandleFields =
        { METHOD_COLUMN, DATE_COLUMN, SHIFT_COLUMN, DEPARTMENT_ASSIGN };

    #endregion

    #region Constructor

    public ExaminationService(ModuleRepository moduleRepository)
    {
        _moduleRepository = moduleRepository;
    }

    #endregion

    #region Public methods

    public static List<ExaminationData> Import(IFormFile file, string examinationId)
    {
        using var wb = new XLWorkbook(file.OpenReadStream());
        var examinationsList = new List<ExaminationData>();

        var ws = wb.Worksheets.FirstOrDefault();

        if (ws == null)
        {
            throw new BadRequestException("Worksheet is empty!");
        }

        var rowsCount = 0;
        var colsCount = 0;
        while (!ws.Cell(rowsCount + 1, 1).Value.IsBlank) rowsCount++;
        while (!ws.Cell(1, colsCount + 1).Value.IsBlank) colsCount++;

        for (var r = 2; r <= rowsCount; r++)
        {
            var row = ws.Row(r);
            var examinationData = new ExaminationData
            {
                ExaminationId = new Guid(examinationId)
            };

            for (var c = 1; c <= colsCount; c++)
            {
                if (ExaminationDataMapping.ContainsKey(c))
                {
                    if (row.Cell(c).IsEmpty())
                    {
                        continue;
                    }

                    var cellValue = row.Cell(c).GetText();
                    var field = ExaminationDataMapping[c];
                    typeof(ExaminationData).GetProperty(field)?.SetValue(examinationData, cellValue);
                    continue;
                }

                if (ExaminationDataIntMapping.ContainsKey(c))
                {
                    if (row.Cell(c).IsEmpty())
                    {
                        continue;
                    }

                    var cellValue = Convert.ToInt32(row.Cell(c).GetDouble());
                    var field = ExaminationDataIntMapping[c];
                    typeof(ExaminationData).GetProperty(field)?.SetValue(examinationData, cellValue);
                    continue;
                }

                if (ExaminationDataHandleFields.Contains(c))
                {
                    if (row.Cell(c).IsEmpty())
                    {
                        continue;
                    }

                    var cellValue = row.Cell(c).Value;

                    switch (c)
                    {
                        case DATE_COLUMN:
                            examinationData.Date = cellValue.GetDateTime();
                            break;
                        case SHIFT_COLUMN:
                            var cv = cellValue.GetText().ToLower();
                            var timeStr = Regex.Matches(cv, @"\d\d")
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
                        case METHOD_COLUMN:
                            examinationData.Method = ExamMethodHelper.FromString(cellValue.GetText().ToLower());
                            break;
                        case DEPARTMENT_ASSIGN:
                            examinationData.DepartmentAssign = cellValue.GetText().ToLower().Equals("bộ môn");
                            break;
                    }
                }
            }

            examinationsList.Add(examinationData);
        }

        return examinationsList;
    }

    public async Task<List<ExaminationData>> ValidateData(IEnumerable<ExaminationData> data)
    {
        var examinationData = data.ToList();

        var modulesTask = _moduleRepository.GetIdsAsync();

        await Task.WhenAll(modulesTask);

        var existedModule = modulesTask.Result.ToDictionary(m => m, _ => true);

        foreach (var row in examinationData)
        {
            var fields = new[]
            {
                "ModuleId", "ModuleName", "ModuleClass", "Credit", "Method", "Date", "StartAt", "EndAt", "Shift",
                "CandidateCount", "RoomsCount", "Rooms", "Faculty", "Department", "DepartmentAssign",
            };
            var acceptNullFields = new[] { "Shift", "Department" };

            foreach (var field in fields)
            {
                if (acceptNullFields.Contains(field))
                {
                    continue;
                }

                var fieldValue = typeof(ExaminationData).GetProperty(field)?.GetValue(row);
                if (fieldValue is null or "")
                {
                    row.Errors.Add(field, "Trường này chưa có giá trị");
                }
            }

            if (row.ModuleId != null && !existedModule.ContainsKey(row.ModuleId))
            {
                row.Errors.Add("ModuleId", "Mã học phần này không tồn tại");
            }
        }

        return examinationData;
    }

    #endregion
}