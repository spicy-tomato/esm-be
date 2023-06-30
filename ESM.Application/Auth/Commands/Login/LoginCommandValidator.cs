using FluentValidation;

namespace ESM.Application.Auth.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}