using ESM.Data.Dtos.Department;
using JetBrains.Annotations;

namespace ESM.Data.Dtos.Faculty;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class FacultyWithDepartments
{
    public Guid Id { get; set; }
    public string? DisplayId { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<DepartmentSimple> Departments { get; set; } = null!;
}