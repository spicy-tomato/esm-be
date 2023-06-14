using FluentValidation;

namespace ESM.Application.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentCommandValidator()
    {
        RuleFor(d => d.DepartmentId)
           .NotEmpty();
        RuleFor(d => d.Name)
           .NotEmpty();
        RuleFor(d => d.FacultyId)
           .NotEmpty();
    }
}