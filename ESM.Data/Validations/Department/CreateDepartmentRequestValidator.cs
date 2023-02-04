using ESM.Data.Request.Department;
using FluentValidation;
using JetBrains.Annotations;

namespace ESM.Data.Validations.Department;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>
{
    public CreateDepartmentRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.SchoolId).NotEmpty();
    }
}