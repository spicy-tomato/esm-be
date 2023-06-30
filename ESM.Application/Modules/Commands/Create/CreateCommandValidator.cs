using FluentValidation;

namespace ESM.Application.Modules.Commands.Create;

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.DisplayId).NotEmpty();
    }
}