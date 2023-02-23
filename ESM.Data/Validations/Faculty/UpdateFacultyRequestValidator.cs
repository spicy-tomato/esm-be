using ESM.Data.Request.Faculty;
using FluentValidation;
using JetBrains.Annotations;

namespace ESM.Data.Validations.Faculty;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UpdateFacultyRequestValidator : AbstractValidator<UpdateFacultyRequest>
{
    public UpdateFacultyRequestValidator()
    {
        RuleFor(x => x.DisplayId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}