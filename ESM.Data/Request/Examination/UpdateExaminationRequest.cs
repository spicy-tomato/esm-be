using JetBrains.Annotations;

namespace ESM.Data.Request.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UpdateExaminationRequest
{
    public string? DisplayId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? ExpectStartAt { get; set; }
    public DateTime? ExpectEndAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}