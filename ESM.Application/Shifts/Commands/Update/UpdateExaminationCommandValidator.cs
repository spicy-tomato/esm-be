using FluentValidation;

namespace ESM.Application.Shifts.Commands.Update;

public class UpdateCommandValidator : AbstractValidator<Examinations.Commands.Update.UpdateCommand>
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