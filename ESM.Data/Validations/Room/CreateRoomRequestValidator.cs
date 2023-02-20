using ESM.Data.Request.Room;
using FluentValidation;
using JetBrains.Annotations;

namespace ESM.Data.Validations.Room;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class CreateRoomRequestValidator : AbstractValidator<CreateRoomRequest>
{
    public CreateRoomRequestValidator()
    {
        RuleFor(x => x.DisplayId).NotEmpty();
    }
}