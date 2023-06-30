using System.Net;
using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using JetBrains.Annotations;
using MediatR;

namespace ESM.Application.Teachers.Commands.Update;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record UpdateCommand(string Id, UpdateRequest Request) : IRequest<Result<bool>>;

public class UpdateCommandHandler : IRequestHandler<UpdateCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IDepartmentService _departmentService;
    private readonly IUserService _userService;

    public UpdateCommandHandler(IApplicationDbContext context,
        IUserService userService,
        IIdentityService identityService,
        IDepartmentService departmentService)
    {
        _context = context;
        _userService = userService;
        _identityService = identityService;
        _departmentService = departmentService;
    }

    public async Task<Result<bool>> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.CheckIfExistAndReturnEntity(request.Id);

        var teacher = _context.Teachers.FirstOrDefault(t => t.Id == user.Id);
        if (teacher is null)
        {
            throw new BadRequestException("This user has not reference teacher");
        }

        Guid? departmentId = request.Request.DepartmentId != null
            ? _departmentService.CheckIfExistAndReturnGuid(request.Request.DepartmentId)
            : null;

        ValidateDuplicatedData(user.Id, request.Request.Email, request.Request.TeacherId);

        // Update teacher data
        teacher.TeacherId = request.Request.TeacherId;
        teacher.FullName = request.Request.FullName;
        teacher.IsMale = request.Request.IsMale;
        teacher.DepartmentId = departmentId;

        // Update user data
        var setMailResult = await _identityService.SetEmailAsync(user, request.Request.Email);

        if (setMailResult.Data)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        return setMailResult;
    }

    private void ValidateDuplicatedData(Guid id, string? email, string? teacherId)
    {
        var errorList = new List<Error>();

        if (email != null && EmailExisted(email, id))
        {
            errorList.Add(new Error("email", "Email"));
        }

        if (teacherId != null && TeacherIdExisted(teacherId, id))
        {
            errorList.Add(new Error("invigilatorId", "Mã CBCT"));
        }

        if (errorList.Count > 0)
        {
            throw new HttpException(HttpStatusCode.Conflict, errorList);
        }
    }

    private bool EmailExisted(string email, Guid id) =>
        _context.Users.FirstOrDefault(u => u.Email == email && u.Id != id) != null;

    private bool TeacherIdExisted(string teacherId, Guid id) =>
        _context.Teachers.FirstOrDefault(u => u.TeacherId == teacherId && u.Id != id) != null;
}