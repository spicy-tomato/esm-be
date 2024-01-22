using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Application.Examinations.Exceptions;
using ESM.Domain.Enums;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ESM.Application.Examinations.Commands.UpdateTeacherAssignment;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record UpdateTeacherAssignmentCommand
    (string Id, string FacultyId, UpdateTeacherAssignmentRequest Request) : IRequest<Result<bool>>;

public class UpdateTeacherAssignmentCommandHandler :
    IRequestHandler<UpdateTeacherAssignmentCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IExaminationService _examinationService;
    private readonly IFacultyService _facultyService;

    public UpdateTeacherAssignmentCommandHandler(IApplicationDbContext context,
        IExaminationService examinationService,
        IFacultyService facultyService)
    {
        _context = context;
        _examinationService = examinationService;
        _facultyService = facultyService;
    }

    public async Task<Result<bool>> Handle(UpdateTeacherAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        var examinationGuid =
            _examinationService.CheckIfExaminationExistAndReturnGuid(request.Id,
                ExaminationStatus.AssignInvigilator);
        var facultyGuid = _facultyService.CheckIfExistAndReturnGuid(request.FacultyId);

        // @formatter:off
        var facultyShiftGroups = _context.DepartmentShiftGroups
            .Where(fg =>
                fg.FacultyShiftGroup.FacultyId == facultyGuid &&
                fg.FacultyShiftGroup.ShiftGroup.ExaminationId == examinationGuid)
            .Include(dg => dg.FacultyShiftGroup)
                .ThenInclude(fg => fg.ShiftGroup)
            .AsEnumerable()
            .ToDictionary(fg => fg.Id.ToString(), fg => fg);
        // @formatter:on

        foreach (var (departmentShiftGroupId, rowData) in request.Request)
        {
            if (!facultyShiftGroups.TryGetValue(departmentShiftGroupId, out var departmentShiftGroup))
            {
                throw new DepartmentGroupNotFoundInFacultyGroupException(departmentShiftGroupId, request.FacultyId);
            }

            if (rowData.DepartmentId != null)
                departmentShiftGroup.DepartmentId = new Guid(rowData.DepartmentId);
            if (rowData.UserId != null)
            {
                departmentShiftGroup.UserId = new Guid(rowData.UserId);
                departmentShiftGroup.TemporaryInvigilatorName = null;
            }
            else if (rowData.TemporaryInvigilatorName != null)
            {
                departmentShiftGroup.TemporaryInvigilatorName = rowData.TemporaryInvigilatorName;
                departmentShiftGroup.UserId = null;
            }
        }

        foreach (var facultyShiftGroup in facultyShiftGroups.Select(fg => fg.Value.FacultyShiftGroup))
        {
            var selectedTeachers = new Dictionary<Guid, bool>();
            foreach (var userId in facultyShiftGroup.DepartmentShiftGroups.Select(dg => dg.UserId))
            {
                if (userId == null)
                {
                    continue;
                }

                if (!selectedTeachers.TryAdd(userId.Value, true))
                {
                    throw new MultipleSelectionTeacherInFacultyGroup(userId.Value, facultyShiftGroup.Id);
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Get(true);
    }
}