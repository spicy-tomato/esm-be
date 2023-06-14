using FluentValidation;

namespace ESM.Application.Departments.Commands.CreateUserInDepartment;

public class CreateUserInDepartmentCommandValidator : AbstractValidator<CreateUserInDepartmentCommand>
{
    public CreateUserInDepartmentCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().Must(u => u.Contains('@'));
        RuleFor(x => x.FullName).NotEmpty();
    }
}