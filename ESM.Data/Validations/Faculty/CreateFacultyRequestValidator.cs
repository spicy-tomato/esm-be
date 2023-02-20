using ESM.Data.Request.Faculty;
using FluentValidation;
using JetBrains.Annotations;

namespace ESM.Data.Validations.Faculty;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class CreateFacultyRequestValidator : AbstractValidator<CreateFacultyRequest>
{
    public CreateFacultyRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}