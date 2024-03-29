using JetBrains.Annotations;

namespace ESM.Data.Request.Department;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class CreateDepartmentRequest
{
    public string? DisplayId { get; set; }
    public string Name { get; set; } = null!;
    public Guid? FacultyId { get; set; }
}