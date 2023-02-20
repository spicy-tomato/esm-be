using ESM.Data.Dtos.Faculty;
using JetBrains.Annotations;

namespace ESM.Data.Dtos.Department;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class DepartmentSummary
{
    public Guid Id { get; set; }
    public string? DisplayId { get; set; }
    public string Name { get; set; } = null!;
    public FacultySummary? Faculty { get; set; }
}