using ESM.Data.Request.School;
using FluentValidation;
using JetBrains.Annotations;

namespace ESM.Data.Validations.School;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class CreateSchoolRequestValidator : AbstractValidator<CreateSchoolRequest>
{
    public CreateSchoolRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}