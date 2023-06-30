using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Helpers;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Data.Enums;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ESM.Application.Examinations.Commands.AutoAssignInvigilatorsToGroups;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record AutoAssignInvigilatorsToGroupsCommand(string Id, string FacultyId) : IRequest<Result<bool>>;

public class AutoAssignInvigilatorsToGroupsCommandHandler
    : IRequestHandler<AutoAssignInvigilatorsToGroupsCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IExaminationService _examinationService;
    private readonly IFacultyService _facultyService;

    public AutoAssignInvigilatorsToGroupsCommandHandler(IApplicationDbContext context,
        IExaminationService examinationService, IFacultyService facultyService)
    {
        _context = context;
        _examinationService = examinationService;
        _facultyService = facultyService;
    }

    public async Task<Result<bool>> Handle(AutoAssignInvigilatorsToGroupsCommand request,
        CancellationToken cancellationToken)
    {
        var examinationGuid =
            _examinationService.CheckIfExaminationExistAndReturnGuid(request.Id, ExaminationStatus.AssignInvigilator);
        var facultyGuid = _facultyService.CheckIfExistAndReturnGuid(request.FacultyId);

        if (_context.Faculties.FirstOrDefault(f => f.Id == facultyGuid) == null)
            throw new NotFoundException("Faculty ID does not exist!");

        var allTeachersInFaculty = _context.Teachers
            .Where(u => u.Department != null && u.Department.FacultyId == facultyGuid)
            .Include(u => u.Department)
            .ToList();

        var shiftGroups = _context.DepartmentShiftGroups
            .Where(dg =>
                dg.FacultyShiftGroup.FacultyId == facultyGuid &&
                dg.FacultyShiftGroup.ShiftGroup.ExaminationId == examinationGuid)
            .Include(dg => dg.FacultyShiftGroup.ShiftGroup)
            .ThenInclude(g => g.Module)
            .OrderBy(dg => dg.FacultyShiftGroup.ShiftGroup.StartAt)
            .ThenBy(dg => dg.FacultyShiftGroup.ShiftGroup.Module.DisplayId)
            .ToList();

        var minimumAppearance = shiftGroups.Count / allTeachersInFaculty.Count;
        var minIndexToRandom = minimumAppearance * allTeachersInFaculty.Count;

        for (var i = 0; i < minIndexToRandom; i++)
        {
            var departmentShiftGroup = shiftGroups[i];
            var invigilatorIndex = i % allTeachersInFaculty.Count;
            var invigilator = allTeachersInFaculty[invigilatorIndex];

            departmentShiftGroup.UserId = invigilator.User.Id;
            departmentShiftGroup.DepartmentId = invigilator.DepartmentId;
        }

        for (var i = minIndexToRandom; i < shiftGroups.Count; i++)
        {
            var departmentShiftGroup = shiftGroups[i];
            var invigilatorIndex = RandomHelper.Next(allTeachersInFaculty.Count);
            var invigilator = allTeachersInFaculty[invigilatorIndex];

            departmentShiftGroup.UserId = invigilator.User.Id;
            departmentShiftGroup.DepartmentId = invigilator.DepartmentId;
            allTeachersInFaculty.RemoveAt(invigilatorIndex);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Get(true);
    }
}