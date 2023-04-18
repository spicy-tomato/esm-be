using JetBrains.Annotations;

namespace ESM.Data.Params.User;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class CheckIfExistParams
{
    public string? InvigilatorId { get; set; }
    public string? Email { get; set; }
}