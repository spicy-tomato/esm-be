using ESM.Data.Request.User;
using FluentValidation;

namespace ESM.Data.Validations.User;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}