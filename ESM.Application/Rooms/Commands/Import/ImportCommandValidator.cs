using FluentValidation;

namespace ESM.Application.Rooms.Commands.Import;

public class ImportCommandValidator : AbstractValidator<ImportCommand>
{
    public ImportCommandValidator()
    {
        RuleFor(e => e.File).NotEmpty();
    }
}