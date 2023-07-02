using System.Diagnostics;
using System.Net;
using ESM.Application.Common.Exceptions.Core;
using ESM.Application.Common.Helpers;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Application.Examinations.Exceptions;
using ESM.Domain.Entities;
using ESM.Domain.Enums;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ESM.Application.Examinations.Commands.ChangeStatus;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record ChangeStatusCommand(string Id, ChangeStatusRequest Request) : IRequest<Result<bool>>;

public class ChangeStatusCommandHandler : IRequestHandler<ChangeStatusCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IExaminationService _examinationService;

    public ChangeStatusCommandHandler(IApplicationDbContext context,
        IExaminationService examinationService)
    {
        _context = context;
        _examinationService = examinationService;
    }

    public async Task<Result<bool>> Handle(ChangeStatusCommand request,
        CancellationToken cancellationToken)
    {
        var entity = _examinationService.CheckIfExaminationExistAndReturnEntity(request.Id);
        var newStatus = request.Request.Status;

        CheckNewStatusIsValid(entity.Status, newStatus);

        switch (entity.Status, newStatus)
        {
            case (ExaminationStatus.Setup, ExaminationStatus.AssignFaculty):
                FinishSetup(entity);
                break;
            case (ExaminationStatus.AssignFaculty, ExaminationStatus.AssignInvigilator):
                FinishAssignFaculty(entity);
                break;
            case (ExaminationStatus.AssignInvigilator, ExaminationStatus.Closed):
                FinishExamination(entity);
                break;
            default:
                throw new ChangedInvalidExaminationStatusException(entity.Status, newStatus);
        }

        _context.ExaminationEvents.Add(new ExaminationEvent
        {
            ExaminationId = entity.Id,
            Status = newStatus
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Get(true);
    }

    private static void CheckNewStatusIsValid(ExaminationStatus currentStatus, ExaminationStatus newStatus)
    {
        if (!Enum.IsDefined(typeof(ExaminationStatus), newStatus))
        {
            throw new UndefinedExaminationStatusException();
        }

        // Key  : Valid current status
        // Value: Valid new status
        var statusMap = new Dictionary<ExaminationStatus, ExaminationStatus[]>
        {
            { ExaminationStatus.Setup, new[] { ExaminationStatus.AssignFaculty } },
            { ExaminationStatus.AssignFaculty, new[] { ExaminationStatus.AssignInvigilator } },
            { ExaminationStatus.AssignInvigilator, new[] { ExaminationStatus.AssignFaculty, ExaminationStatus.Closed } }
        };

        foreach (var (validCurrentStatus, validNewStatuses) in statusMap)
        {
            if (currentStatus == validCurrentStatus && validNewStatuses.Contains(newStatus))
            {
                return;
            }
        }

        var expectedCurrentStatusSatisfiesNewStatus = statusMap
            .Where(item =>
                item.Value.Contains(newStatus))
            .ToList();

        if (expectedCurrentStatusSatisfiesNewStatus.IsNullOrEmpty())
        {
            throw new ChangedInvalidExaminationStatusException(newStatus, currentStatus);
        }

        throw new ChangedInvalidExaminationStatusException(expectedCurrentStatusSatisfiesNewStatus.Select(s => s.Key));
    }

    private void FinishSetup(Examination entity)
    {
        var data = _context.ExaminationData.Where(e => e.ExaminationId == entity.Id);
        if (data.Any())
        {
            data = ValidateTemporaryData(data);
        }

        var modules = _context.Modules.AsNoTracking();
        var modulesDictionary = modules.ToDictionary(m => m.DisplayId, m => m.Id);

        var rooms = _context.Rooms.AsNoTracking();
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
                    ExaminationId = entity.Id,
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
                {
                    candidatesNumberInShift++;
                }

                var invigilatorsCount = CalculateInvigilatorNumber(candidatesNumberInShift);
                var invigilatorShift = new List<InvigilatorShift>();

                for (var j = 1; j <= invigilatorsCount; j++)
                {
                    invigilatorShift.Add(new InvigilatorShift { OrderIndex = j });
                }

                shifts.Add(new Shift
                {
                    ExamsCount = CalculateExamsNumber(shift.Method, candidatesNumberInShift),
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
        {
            shiftGroup.Value.InvigilatorsCount++;
        }

        _context.Shifts.AddRange(shifts);

        entity.Status = ExaminationStatus.AssignFaculty;
    }

    private static int CalculateExamsNumber(ExamMethod? method, int candidatesNumberInShift)
    {
        if (method != ExamMethod.Write)
        {
            return 0;
        }

        var shouldRoundUp = candidatesNumberInShift % 5 != 0;
        return (candidatesNumberInShift / 5 + (shouldRoundUp ? 1 : 0)) * 5;
    }

    private void FinishAssignFaculty(Examination entity)
    {
        _context.Entry(entity)
            .Collection(e => e.ShiftGroups)
            .Query()
            .Include(eg => eg.FacultyShiftGroups)
            .Include(eg => eg.Module)
            .Where(eg => !eg.DepartmentAssign)
            .Load();

        foreach (var group in entity.ShiftGroups)
        {
            var expected = group.InvigilatorsCount;
            var actual = group.FacultyShiftGroups.Sum(feg => feg.InvigilatorsCount);
            if (actual != expected)
            {
                throw new ActualAssignedInvigilatorsNumberNotMatchException(actual, expected, group);
            }
        }

        var oldDepartmentShiftGroups =
            _context.DepartmentShiftGroups.Where(dg => dg.FacultyShiftGroup.ShiftGroup.ExaminationId == entity.Id);
        _context.DepartmentShiftGroups.RemoveRange(oldDepartmentShiftGroups);

        foreach (var group in entity.ShiftGroups)
        {
            foreach (var facultyGroup in group.FacultyShiftGroups)
            {
                facultyGroup.DepartmentShiftGroups =
                    new List<DepartmentShiftGroup>(new DepartmentShiftGroup[facultyGroup.InvigilatorsCount])
                        .Select(_ => new DepartmentShiftGroup { FacultyShiftGroup = facultyGroup })
                        .ToList();
            }
        }

        entity.Status = ExaminationStatus.AssignInvigilator;
    }

    private void FinishExamination(Examination entity)
    {
        _context.Entry(entity)
            .Collection(e => e.ShiftGroups)
            .Query()
            .Include(eg => eg.Shifts)
            .ThenInclude(s => s.Room)
            .Where(eg => !eg.DepartmentAssign)
            .Load();

        var shifts = entity.ShiftGroups.SelectMany(sg => sg.Shifts);
        var notHandedOverShifts = shifts.Where(s => s.HandedOverUserId == null).ToList();

        if (notHandedOverShifts.Any())
        {
            var errorList = notHandedOverShifts.Select(s =>
                new Error(
                    HttpStatusCode.BadRequest,
                    $"Shift have not been handed over yet: Date {s.ShiftGroup.StartAt}, module {s.ShiftGroup.ModuleId}, room {s.Room.DisplayId}")
            );
            throw new HttpException(HttpStatusCode.Conflict, errorList);
        }


        entity.Status = ExaminationStatus.Closed;
    }

    /// <summary>
    /// Validate temporary data
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private IQueryable<ExaminationData> ValidateTemporaryData(IEnumerable<ExaminationData> data)
    {
        var examinationData = data.ToList();

        var existedModules = _context.Modules.Select(m => m.DisplayId).ToDictionary(m => m, _ => true);
        var existedRooms = _context.Rooms.Select(m => m.DisplayId).ToDictionary(m => m, _ => true);

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

    /// <summary>
    /// Validate module ID
    /// </summary>
    /// <param name="row"></param>
    /// <param name="existedModules"></param>
    private static void ValidateModuleId(ExaminationData row, IReadOnlyDictionary<string, bool> existedModules)
    {
        if (row.ModuleId != null && !existedModules.ContainsKey(row.ModuleId))
        {
            row.Errors.Add("moduleId", new ExaminationDataError("Mã học phần này không tồn tại"));
        }
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

        var notExistedRooms = rooms
            .Where(room => !existedRooms.ContainsKey(room))
            .ToList();

        if (notExistedRooms.Count > 0)
            row.Errors.Add("rooms",
                new ExaminationDataError<string>(
                    "Các phòng thi sau không tồn tại: " + string.Join(", ", notExistedRooms),
                    notExistedRooms)
            );
    }

    private static int CalculateInvigilatorNumber(int candidatesCount)
    {
        return candidatesCount < 80 ? 2 : 3;
    }
}