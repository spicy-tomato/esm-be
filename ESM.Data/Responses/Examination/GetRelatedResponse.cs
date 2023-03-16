using JetBrains.Annotations;

namespace ESM.Data.Responses.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class GetRelatedResponseItem
{
    public Guid Id { get; set; }
    public string DisplayId { get; set; } = null!;
    public string Name { get; set; } = null!;
}