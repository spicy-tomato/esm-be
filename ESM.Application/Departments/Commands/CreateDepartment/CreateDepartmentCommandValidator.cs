using FluentValidation;

namespace ESM.Application.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(d => d.Name)
           .MaximumLength(200)
           .NotEmpty();
    }
}