using JetBrains.Annotations;

namespace ESM.Data.Request.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class CreateExaminationRequest
{
    public string? DisplayId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? ExpectStartAt { get; set; }
    public DateTime? ExpectEndAt { get; set; }
}