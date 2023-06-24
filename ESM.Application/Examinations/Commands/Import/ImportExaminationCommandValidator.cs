using FluentValidation;

namespace ESM.Application.Examinations.Commands.Import;

public class ImportCommandValidator : AbstractValidator<ImportCommand>
{
    public ImportCommandValidator()
    {
        RuleFor(e => e.ExaminationId).NotEmpty();
        RuleFor(e => e.File).NotEmpty();
        RuleFor(e => e.CreatedAt).NotEmpty();
    }
}