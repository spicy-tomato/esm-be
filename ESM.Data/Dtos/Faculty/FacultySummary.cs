using ESM.Data.Dtos.School;
using JetBrains.Annotations;

namespace ESM.Data.Dtos.Faculty;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class FacultySummary
{
    public Guid Id { get; set; }
    public string? DisplayId { get; set; }
    public string Name { get; set; } = null!;
    public SchoolSummary SchoolSummary { get; set; } = null!;
}