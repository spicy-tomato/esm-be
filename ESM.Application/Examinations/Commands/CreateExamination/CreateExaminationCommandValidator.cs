using FluentValidation;

namespace ESM.Application.Examinations.Commands.CreateExamination;

public class CreateExaminationCommandValidator : AbstractValidator<CreateExaminationCommand>
{
    public CreateExaminationCommandValidator()
    {
        RuleFor(e => e.Name).NotEmpty();
        RuleFor(e => e).Must(x =>
            x.ExpectStartAt == null ||
            x.ExpectEndAt == null ||
            DateTime.Compare(x.ExpectStartAt.Value, x.ExpectEndAt.Value) < 0
        );
    }
}