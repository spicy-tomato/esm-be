using JetBrains.Annotations;

namespace ESM.Data.Dtos.School;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class SchoolSummary
{
    public Guid Id { get; set; }
    public string? DisplayId { get; set; }
    public string Name { get; set; } = null!;
}