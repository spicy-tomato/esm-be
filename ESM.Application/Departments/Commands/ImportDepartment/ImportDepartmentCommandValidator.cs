using FluentValidation;

namespace ESM.Application.Departments.Commands.ImportDepartment;

public class ImportDepartmentCommandValidator : AbstractValidator<ImportDepartmentCommand>
{
    public ImportDepartmentCommandValidator()
    {
        RuleFor(d => d.Files)
           .NotEmpty();
    }
}