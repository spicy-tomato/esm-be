using FluentValidation;

namespace ESM.Application.Modules.Commands.Import;

public class ImportCommandValidator : AbstractValidator<ImportCommand>
{
    public ImportCommandValidator()
    {
        RuleFor(e => e.Files)
            .NotEmpty();
    }
}