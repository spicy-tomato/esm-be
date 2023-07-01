using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Domain.Identity;
using JetBrains.Annotations;
using MediatR;

namespace ESM.Application.Groups.Commands.UpdateTemporaryNameToTeacher;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record UpdateTemporaryNameToTeacherCommand(string Id, string UserId) : IRequest<Result<bool>>;

public class UpdateTemporaryNameToTeacherCommandHandler
    : IRequestHandler<UpdateTemporaryNameToTeacherCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IGroupService _groupService;
    private readonly IUserService _userService;

    public UpdateTemporaryNameToTeacherCommandHandler(IApplicationDbContext context, IGroupService groupService,
        IUserService userService)
    {
        _context = context;
        _groupService = groupService;
        _userService = userService;
    }

    public async Task<Result<bool>> Handle(UpdateTemporaryNameToTeacherCommand request,
        CancellationToken cancellationToken)
    {
        if (!_userService.UserExist(request.UserId))
        {
            throw new NotFoundException(nameof(ApplicationUser), request.UserId);
        }

        var userGuid = Guid.Parse(request.UserId);
        var groupGuid = _groupService.CheckIfGroupExistAndReturnGuid(request.Id);

        var departmentShiftGroup = _context.DepartmentShiftGroups
            .FirstOrDefault(dsg => dsg.FacultyShiftGroup.ShiftGroupId == groupGuid);

        if (departmentShiftGroup is null)
        {
            throw new BadRequestException("Data is invalid");
        }

        departmentShiftGroup.UserId = userGuid;
        departmentShiftGroup.TemporaryInvigilatorName = null;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Get(true);
    }
}