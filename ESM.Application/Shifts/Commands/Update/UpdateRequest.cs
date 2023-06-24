using JetBrains.Annotations;

namespace ESM.Application.Shifts.Commands.Update;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UpdateRequest
{
    public string? HandoverTeacherId { get; set; }
    public string? Report { get; set; }
}