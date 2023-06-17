using FluentValidation;

namespace ESM.Application.Examinations.Commands.ImportExamination;

public class ImportExaminationCommandValidator : AbstractValidator<ImportExaminationCommand>
{
    public ImportExaminationCommandValidator()
    {
        RuleFor(e => e.ExaminationId).NotEmpty();
        RuleFor(e => e.File).NotEmpty();
        RuleFor(e => e.CreatedAt).NotEmpty();
    }
}