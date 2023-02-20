using JetBrains.Annotations;

namespace ESM.Data.Dtos.Department;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class DepartmentSimple
{
    public Guid Id { get; set; }
    public string? DisplayId { get; set; }
    public string Name { get; set; } = null!;
}