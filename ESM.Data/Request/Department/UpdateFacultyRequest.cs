using JetBrains.Annotations;

namespace ESM.Data.Request.Department;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UpdateDepartmentRequest
{
    public string DisplayId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string FacultyId { get; set; } = null!;
}