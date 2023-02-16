using ESM.Data.Request.Examination;
using FluentValidation;
using JetBrains.Annotations;

namespace ESM.Data.Validations.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class CreateExaminationRequestValidator : AbstractValidator<CreateExaminationRequest>
{
    public CreateExaminationRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x).Must(x =>
            x.ExpectStartAt == null ||
            x.ExpectEndAt == null ||
            DateTime.Compare(x.ExpectStartAt.Value, x.ExpectEndAt.Value) < 0
        );
    }
}