using JetBrains.Annotations;

namespace ESM.Data.Request.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UpdateShiftRequest
{
    public string? HandoverUserId { get; set; }
    public string? Report { get; set; }
}