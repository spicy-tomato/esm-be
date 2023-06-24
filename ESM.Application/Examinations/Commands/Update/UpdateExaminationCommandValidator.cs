using FluentValidation;

namespace ESM.Application.Examinations.Commands.Update;

public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(e => new { e.ExpectStartAt, e.ExpectEndAt })
           .Must(e =>
                e.ExpectStartAt == null ||
                e.ExpectEndAt == null ||
                e.ExpectStartAt < e.ExpectEndAt);
    }
}