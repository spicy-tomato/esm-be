using ESM.Data.Request.Module;
using FluentValidation;
using JetBrains.Annotations;

namespace ESM.Data.Validations.Module;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class CreateModuleRequestValidator : AbstractValidator<CreateModuleRequest>
{
    public CreateModuleRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.DisplayId).NotEmpty();
    }
}