using System.ComponentModel.DataAnnotations;
using ESM.Domain.Common;
using JetBrains.Annotations;

namespace ESM.Domain.Entities;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Faculty: BaseAuditableEntity
{
    [MaxLength(20)]
    public string? DisplayId { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    public ICollection<Department> Departments { get; set; } = new List<Department>();

    public ICollection<FacultyShiftGroup> FacultyShiftGroups { get; set; } = new List<FacultyShiftGroup>();
}