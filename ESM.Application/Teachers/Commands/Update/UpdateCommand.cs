using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Application.Teachers.Exceptions;
using ESM.Application.Users.Exceptions;
using MediatR;

namespace ESM.Application.Teachers.Commands.Update;

public record UpdateCommand(string Id, UpdateRequest Request) : IRequest<Result<bool>>;

public class UpdateCommandHandler : IRequestHandler<UpdateCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IDepartmentService _departmentService;

    public UpdateCommandHandler(IApplicationDbContext context,
        IIdentityService identityService,
        IDepartmentService departmentService)
    {
        _context = context;
        _identityService = identityService;
        _departmentService = departmentService;
    }

    public async Task<Result<bool>> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindUserByIdAsync(request.Id);
        if (user is null)
        {
            throw new UserNotFoundException(request.Id);
        }

        var teacher = _context.Teachers.FirstOrDefault(t => t.Id == user.Id);
        if (teacher is null)
        {
            throw new UserHaveNoReferenceTeacherException();
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
            throw new ConflictTeacherDataException(errorList);
        }
    }

    private bool EmailExisted(string email, Guid id) =>
        _context.Users.FirstOrDefault(u => u.Email == email && u.Id != id) != null;

    private bool TeacherIdExisted(string teacherId, Guid id) =>
        _context.Teachers.FirstOrDefault(u => u.TeacherId == teacherId && u.Id != id) != null;
}