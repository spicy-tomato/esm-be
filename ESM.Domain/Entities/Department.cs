using System.ComponentModel.DataAnnotations;
using ESM.Domain.Common;
using JetBrains.Annotations;

namespace ESM.Domain.Entities;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Department : BaseAuditableEntity
{
    [MaxLength(20)]
    public string? DisplayId { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public Guid? FacultyId { get; set; }
    public Faculty? Faculty { get; set; }

    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

    public ICollection<DepartmentShiftGroup> DepartmentShiftGroups { get; set; } = new List<DepartmentShiftGroup>();
}