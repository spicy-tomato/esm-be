using JetBrains.Annotations;

namespace ESM.Domain.Dtos.Faculty;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class FacultySummary
{
    public Guid Id { get; set; }
    public string? DisplayId { get; set; }
    public string Name { get; set; } = null!;
}