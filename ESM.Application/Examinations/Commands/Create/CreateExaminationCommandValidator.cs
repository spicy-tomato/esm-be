using FluentValidation;

namespace ESM.Application.Examinations.Commands.Create;

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(e => e.Name).NotEmpty();
        RuleFor(e => e).Must(x =>
            x.ExpectStartAt == null ||
            x.ExpectEndAt == null ||
            DateTime.Compare(x.ExpectStartAt.Value, x.ExpectEndAt.Value) < 0
        );
    }
}