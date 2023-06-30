using FluentValidation;

namespace ESM.Application.Faculties.Commands.Create;

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(e => e.Name).NotEmpty();
    }
}