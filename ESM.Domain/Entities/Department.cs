using ESM.Domain.Common;
using JetBrains.Annotations;

namespace ESM.Domain.Entities;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Department : BaseAuditableEntity
{
    public string? DisplayId { get; set; }

    public string Name { get; set; } = null!;

    public Guid? FacultyId { get; set; }
    public Faculty? Faculty { get; set; }

    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

    public ICollection<DepartmentShiftGroup> DepartmentShiftGroups { get; set; } = new List<DepartmentShiftGroup>();
}