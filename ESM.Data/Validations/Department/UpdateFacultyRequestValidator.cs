using ESM.Data.Request.Department;
using FluentValidation;
using JetBrains.Annotations;

namespace ESM.Data.Validations.Department;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UpdateDepartmentRequestValidator : AbstractValidator<UpdateDepartmentRequest>
{
    public UpdateDepartmentRequestValidator()
    {
        RuleFor(x => x.DisplayId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.FacultyId).NotEmpty();
    }
}