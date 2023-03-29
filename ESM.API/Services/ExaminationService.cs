using System.Diagnostics;
using ClosedXML.Excel;
using ESM.Common.Core.Exceptions;
using ESM.Data.Models;
using System.Text.RegularExpressions;
using ESM.API.Repositories.Implementations;
using ESM.Common.Core.Helpers;
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
    private readonly RoomRepository _roomRepository;

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
        { METHOD_COLUMN, DATE_COLUMN, SHIFT_COLUMN, DEPARTMENT_ASSIGN };

    #endregion

    #region Constructor

    public ExaminationService(ModuleRepository moduleRepository, RoomRepository roomRepository)
    {
        _moduleRepository = moduleRepository;
        _roomRepository = roomRepository;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Import data
    /// </summary>
    /// <param name="file"></param>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    public static List<ExaminationData> Import(IFormFile file, string examinationId)
    {
        using var wb = new XLWorkbook(file.OpenReadStream());
        var examinationsList = new List<ExaminationData>();

        var ws = wb.Worksheets.FirstOrDefault();
        if (ws == null)
            throw new BadRequestException("Worksheet is empty!");

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
                        continue;

                    var cellValue = row.Cell(c).GetText().Trim();
                    var field = ExaminationDataMapping[c];
                    typeof(ExaminationData).GetProperty(field)?.SetValue(examinationData, cellValue);
                    continue;
                }

                if (ExaminationDataIntMapping.ContainsKey(c))
                {
                    if (row.Cell(c).IsEmpty())
                        continue;

                    var cellValue = Convert.ToInt32(row.Cell(c).GetDouble());
                    var field = ExaminationDataIntMapping[c];
                    typeof(ExaminationData).GetProperty(field)?.SetValue(examinationData, cellValue);
                    continue;
                }

                if (ExaminationDataHandleFields.Contains(c))
                {
                    if (row.Cell(c).IsEmpty())
                        continue;

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

    /// <summary>
    /// Validate temporary data
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public IQueryable<ExaminationData> ValidateData(IEnumerable<ExaminationData> data)
    {
        var examinationData = data.ToList();

        var existedModules = _moduleRepository.GetIds().ToDictionary(m => m, _ => true);
        var existedRooms = _roomRepository.GetIds().ToDictionary(m => m, _ => true);

        foreach (var row in examinationData)
        {
            var fields = new[]
            {
                "ModuleId", "ModuleName", "ModuleClass", "Credit", "Method", "Date", "StartAt", "EndAt", "Shift",
                "CandidatesCount", "RoomsCount", "Rooms", "Faculty", "Department", "DepartmentAssign"
            };
            var acceptNullFields = new[] { "Shift", "Department" };

            foreach (var field in fields)
            {
                if (acceptNullFields.Contains(field))
                    continue;

                var fieldValue = typeof(ExaminationData).GetProperty(field)?.GetValue(row);
                var normalizeField = string.Concat(field[..1].ToLower(), field[1..]);

                if (fieldValue is null or "")
                    row.Errors.Add(normalizeField, new ExaminationDataError("Trường này chưa có giá trị"));
            }

            ValidateModuleId(row, existedModules);
            ValidateRoom(row, existedRooms);
        }

        return examinationData.AsQueryable();
    }

    public IEnumerable<Shift> RetrieveShiftsFromTemporaryData(Guid examinationGuid,
        IEnumerable<ExaminationData> data)
    {
        var modules = _moduleRepository.GetAll();
        var modulesDictionary = modules.ToDictionary(m => m.DisplayId, m => m.Id);

        var rooms = _roomRepository.GetAll();
        var roomsDictionary = rooms.ToDictionary(m => m.DisplayId, m => m.Id);

        var shiftGroupsDictionary = new Dictionary<string, ShiftGroup>();
        var shifts = new List<Shift>();

        foreach (var shift in data)
        {
            Debug.Assert(shift.StartAt != null, "shift.StartAt != null");
            Debug.Assert(shift.Method != null, "shift.Method != null");
            Debug.Assert(shift.ModuleId != null, "shift.ModuleId != null");
            Debug.Assert(shift.CandidatesCount != null, "shift.CandidatesCount != null");

            var roomsInShift = RoomHelper.GetRoomsFromString(shift.Rooms);
            var shiftGroupKey = string.Join('_',
                shift.ModuleId,
                shift.Method.ToString(),
                shift.StartAt.Value.ToShortDateString(),
                shift.Shift.ToString() ?? "null"
            );

            if (!shiftGroupsDictionary.TryGetValue(shiftGroupKey, out var shiftGroup))
            {
                shiftGroup = new ShiftGroup
                {
                    Id = Guid.NewGuid(),
                    Method = shift.Method.Value,
                    InvigilatorsCount = 0,
                    RoomsCount = 0,
                    StartAt = shift.StartAt.Value,
                    Shift = shift.Shift,
                    DepartmentAssign = shift.DepartmentAssign ?? false,
                    ExaminationId = examinationGuid,
                    ModuleId = modulesDictionary[shift.ModuleId]
                };
                shiftGroupsDictionary.Add(shiftGroupKey, shiftGroup);
            }

            var minCandidatesNumberInShift = shift.CandidatesCount.Value / roomsInShift.Length;
            var remainder = shift.CandidatesCount.Value % roomsInShift.Length;

            for (var i = 0; i < roomsInShift.Length; i++)
            {
                var room = roomsInShift[i];
                var candidatesNumberInShift = minCandidatesNumberInShift;
                if (0 < remainder && remainder <= i + 1)
                    candidatesNumberInShift++;

                var invigilatorsCount = ExaminationHelper.CalculateInvigilatorNumber(candidatesNumberInShift);
                var invigilatorShift = new List<InvigilatorShift>();

                for (var j = 1; j <= invigilatorsCount; j++)
                {
                    invigilatorShift.Add(new InvigilatorShift { OrderIndex = j });
                }

                shifts.Add(new Shift
                {
                    ExamsCount = ExaminationHelper.CalculateExamsNumber(shift),
                    CandidatesCount = candidatesNumberInShift,
                    InvigilatorsCount = invigilatorsCount,
                    RoomId = roomsDictionary[room],
                    ShiftGroup = shiftGroup,
                    InvigilatorShift = invigilatorShift
                });

                shiftGroup.InvigilatorsCount += invigilatorsCount;
            }

            shiftGroup.RoomsCount += roomsInShift.Length;
        }

        // +1 invigilator for each shift
        foreach (var shiftGroup in shiftGroupsDictionary)
            shiftGroup.Value.InvigilatorsCount++;

        return shifts;
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Validate module ID
    /// </summary>
    /// <param name="row"></param>
    /// <param name="existedModules"></param>
    private static void ValidateModuleId(ExaminationData row, IReadOnlyDictionary<string, bool> existedModules)
    {
        if (row.ModuleId != null && !existedModules.ContainsKey(row.ModuleId))
            row.Errors.Add("moduleId", new ExaminationDataError("Mã học phần này không tồn tại"));
    }

    /// <summary>
    /// Validate rooms
    /// </summary>
    /// <param name="row"></param>
    /// <param name="existedRooms"></param>
    private static void ValidateRoom(ExaminationData row, IReadOnlyDictionary<string, bool> existedRooms)
    {
        var rooms = RoomHelper.GetRoomsFromString(row.Rooms);
        if (rooms.Length == 0)
        {
            row.Errors.Add("rooms", new ExaminationDataError("Chưa có phòng thi"));
            return;
        }

        var notExistedRooms = new List<string>();
        foreach (var room in rooms)
        {
            if (existedRooms.ContainsKey(room))
                continue;

            notExistedRooms.Add(room);

            // if (!row.Suggestions.ContainsKey("rooms"))
            //     row.Suggestions.Add("rooms", new List<KeyValuePair<string, string>>());
            // row.Suggestions["rooms"].Add(new KeyValuePair<string, string>(room, shortenName));
        }

        if (notExistedRooms.Count > 0)
            row.Errors.Add("rooms",
                new ExaminationDataError<string>(
                    "Các phòng thi sau không tồn tại: " + string.Join(", ", notExistedRooms),
                    notExistedRooms)
            );
    }

    #endregion
}