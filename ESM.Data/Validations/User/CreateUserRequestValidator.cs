using ESM.Data.Request.User;
using FluentValidation;
using JetBrains.Annotations;

namespace ESM.Data.Validations.User;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().Must(u => u.Contains('@'));
        RuleFor(x => x.FullName).NotEmpty();
    }
}