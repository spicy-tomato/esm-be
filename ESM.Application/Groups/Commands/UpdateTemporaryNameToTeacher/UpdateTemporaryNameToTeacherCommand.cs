using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Application.Groups.Exceptions;
using ESM.Application.Users.Exceptions;
using MediatR;

namespace ESM.Application.Groups.Commands.UpdateTemporaryNameToTeacher;

public record UpdateTemporaryNameToTeacherCommand(string Id, string UserId) : IRequest<Result<bool>>;

public class UpdateTemporaryNameToTeacherCommandHandler
    : IRequestHandler<UpdateTemporaryNameToTeacherCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IGroupService _groupService;

    public UpdateTemporaryNameToTeacherCommandHandler(IApplicationDbContext context,
        IGroupService groupService,
        IIdentityService identityService)
    {
        _context = context;
        _groupService = groupService;
        _identityService = identityService;
    }

    public async Task<Result<bool>> Handle(UpdateTemporaryNameToTeacherCommand request,
        CancellationToken cancellationToken)
    {
        if (await _identityService.FindUserByIdAsync(request.UserId) is null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        var userGuid = Guid.Parse(request.UserId);
        var groupGuid = _groupService.CheckIfGroupExistAndReturnGuid(request.Id);

        var departmentShiftGroup = _context.DepartmentShiftGroups
            .FirstOrDefault(dsg => dsg.FacultyShiftGroup.ShiftGroupId == groupGuid);

        if (departmentShiftGroup is null)
        {
            throw new ShiftGroupNotFoundException(groupGuid);
        }

        departmentShiftGroup.UserId = userGuid;
        departmentShiftGroup.TemporaryInvigilatorName = null;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Get(true);
    }
}