using ESM.Data.Request.User;
using FluentValidation;
using JetBrains.Annotations;

namespace ESM.Data.Validations.User;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().Must(u => u.Contains('@'));
        RuleFor(x => x.FullName).NotEmpty();
        RuleFor(x => x.IsMale).NotNull();
    }
}