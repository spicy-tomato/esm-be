using FluentValidation;

namespace ESM.Application.Rooms.Commands.Create;

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(x => x.DisplayId).NotEmpty();
    }
}