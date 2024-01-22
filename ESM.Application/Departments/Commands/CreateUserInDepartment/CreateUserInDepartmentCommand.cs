using System.Net;
using ESM.Application.Common.Exceptions.Core;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Application.Departments.Exceptions;
using ESM.Application.Users.Exceptions;
using MediatR;

namespace ESM.Application.Departments.Commands.CreateUserInDepartment;

public record CreateUserInDepartmentParams(
    string Email,
    string? TeacherId,
    string FullName,
    bool IsMale,
    string? PhoneNumber
) : IRequest<Result<Guid>>;

public record CreateUserInDepartmentCommand(
    string Email,
    string? TeacherId,
    string FullName,
    bool IsMale,
    string? PhoneNumber,
    string DepartmentId) : IRequest<Result<Guid>>
{
    public CreateUserInDepartmentCommand(CreateUserInDepartmentParams @params, string DepartmentId)
        : this(@params.Email, @params.TeacherId, @params.FullName, @params.IsMale, @params.PhoneNumber, DepartmentId) { }
}

public class CreateUserInDepartmentCommandHandler : IRequestHandler<CreateUserInDepartmentCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public CreateUserInDepartmentCommandHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Result<Guid>> Handle(CreateUserInDepartmentCommand request, CancellationToken cancellationToken)
    {
        var guid = Guid.Parse(request.DepartmentId);
        var entity = await _context.Departments.FindAsync(new object[] { guid }, cancellationToken);

        if (entity == null)
        {
            throw new DepartmentNotFoundException(guid);
        }

        ValidateDuplicatedData(guid, request);

        var result = await _identityService.CreateUserAsync(request.Email, request.Email);
        if (!result.Result.Success)
        {
            throw new UserCreationException();
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Get(result.UserId);
    }

    private void ValidateDuplicatedData(Guid id, CreateUserInDepartmentCommand request)
    {
        var errorList = new List<Error>();
        var usersWithDuplicatedData = _context.Teachers.Where(
            u => u.Id != id &&
                 ((u.User.Email != null && u.User.Email == request.Email) ||
                  (u.TeacherId != null && u.TeacherId != request.TeacherId))
        );

        foreach (var u in usersWithDuplicatedData)
        {
            if (u.User.Email != null && u.User.Email == request.Email)
                errorList.Add(new Error("email", "Email"));
            if (u.TeacherId != null && u.TeacherId == request.TeacherId)
                errorList.Add(new Error("invigilatorId", "Mã CBCT"));
        }

        if (errorList.Count > 0)
        {
            throw new HttpException(HttpStatusCode.Conflict, errorList);
        }
    }
}