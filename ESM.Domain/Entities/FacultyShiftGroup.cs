using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ESM.Domain.Entities;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class FacultyShiftGroup
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public int InvigilatorsCount { get; set; }
    public int CalculatedInvigilatorsCount { get; set; }

    public Guid FacultyId { get; set; }
    public Faculty Faculty { get; set; } = null!;

    public Guid ShiftGroupId { get; set; }
    public ShiftGroup ShiftGroup { get; set; } = null!;

    public ICollection<DepartmentShiftGroup> DepartmentShiftGroups { get; set; } = new List<DepartmentShiftGroup>();
}