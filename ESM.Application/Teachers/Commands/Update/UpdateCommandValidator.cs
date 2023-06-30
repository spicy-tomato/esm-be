using FluentValidation;

namespace ESM.Application.Teachers.Commands.Update;

public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(x => x.Request.Email).NotEmpty().Must(u => u.Contains('@'));
        RuleFor(x => x.Request.FullName).NotEmpty();
        RuleFor(x => x.Request.IsMale).NotNull();
    }
}