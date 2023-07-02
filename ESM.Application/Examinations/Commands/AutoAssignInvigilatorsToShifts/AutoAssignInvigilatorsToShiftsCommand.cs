using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Application.Groups.Exceptions;
using ESM.Domain.Entities;
using ESM.Domain.Enums;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ESM.Application.Examinations.Commands.AutoAssignInvigilatorsToShifts;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record AutoAssignInvigilatorsToShiftsCommand(string Id) : IRequest<Result<bool>>;

public class
    AutoAssignInvigilatorsToShiftsCommandHandler : IRequestHandler<AutoAssignInvigilatorsToShiftsCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IExaminationService _examinationService;

    public AutoAssignInvigilatorsToShiftsCommandHandler(IApplicationDbContext context,
        IExaminationService examinationService)
    {
        _context = context;
        _examinationService = examinationService;
    }

    public async Task<Result<bool>> Handle(AutoAssignInvigilatorsToShiftsCommand request,
        CancellationToken cancellationToken)
    {
        var examinationGuid =
            _examinationService.CheckIfExaminationExistAndReturnGuid(request.Id, ExaminationStatus.AssignInvigilator);

        // @formatter:off
        var departmentShiftGroups = _context.DepartmentShiftGroups
           .Where(dg =>
                dg.FacultyShiftGroup.ShiftGroup.ExaminationId == examinationGuid &&
                !dg.FacultyShiftGroup.ShiftGroup.DepartmentAssign)
           .Include(dg => dg.FacultyShiftGroup)
               .ThenInclude(fg => fg.ShiftGroup)
               .ThenInclude(g => g.Module)
           .Include(dg => dg.User)
               .ThenInclude(u => u!.Teacher!.Department)
           .OrderBy(dg => dg.FacultyShiftGroup.ShiftGroup.StartAt)
               .ThenBy(dg => dg.FacultyShiftGroup.ShiftGroup.Module.DisplayId)
           .ToList();
        // @formatter:on

        var departmentShiftGroupsDict = new Dictionary<string, List<DepartmentShiftGroup>>();
        foreach (var dg in departmentShiftGroups)
        {
            var key =
                dg.FacultyShiftGroup.ShiftGroup.StartAt.ToString("yy-MM-dd:hh:mm:ss") +
                dg.FacultyShiftGroup.ShiftGroup.Module.DisplayId;

            if (!departmentShiftGroupsDict.ContainsKey(key))
            {
                departmentShiftGroupsDict.Add(key, new List<DepartmentShiftGroup>());
            }

            departmentShiftGroupsDict[key].Add(dg);
        }

        // @formatter:off
        var invigilatorShift = _context.InvigilatorShift
           .Where(i => i.Shift.ShiftGroup.ExaminationId == examinationGuid && !i.Shift.ShiftGroup.DepartmentAssign)
           .Include(i => i.Shift)
               .ThenInclude(s => s.ShiftGroup)
               .ThenInclude(g => g.Module)
           .ToList();
        // @formatter:on

        foreach (var ivs in invigilatorShift)
        {
            var shiftGroupId =
                ivs.Shift.ShiftGroup.StartAt.ToString("yy-MM-dd:hh:mm:ss") +
                ivs.Shift.ShiftGroup.Module.DisplayId;

            if (!departmentShiftGroupsDict.TryGetValue(shiftGroupId, out var invigilatorsBucket))
            {
                throw new ShiftGroupNotFoundInInvigilatorShiftException(shiftGroupId, ivs.Id);
            }

            var isPrioritySlot = ivs.OrderIndex == 1;
            var priorityFacultyId = ivs.Shift.ShiftGroup.Module.FacultyId;

            for (var i = 0; i < invigilatorsBucket.Count; i++)
            {
                var departmentShiftGroup = invigilatorsBucket[i];
                var facultyOfModuleSamePriorityFaculty =
                    departmentShiftGroup.User?.Teacher?.Department?.FacultyId == priorityFacultyId;

                if ((isPrioritySlot && !facultyOfModuleSamePriorityFaculty) ||
                    (!isPrioritySlot && facultyOfModuleSamePriorityFaculty))
                {
                    continue;
                }

                ivs.InvigilatorId = departmentShiftGroup.UserId;
                invigilatorsBucket.RemoveAt(i);
                break;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Get(true);
    }
}