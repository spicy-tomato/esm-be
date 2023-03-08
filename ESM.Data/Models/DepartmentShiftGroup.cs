using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class DepartmentShiftGroup
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public int InvigilatorsCount { get; set; }

    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public Guid FacultyShiftGroupId { get; set; }
    public FacultyShiftGroup FacultyShiftGroup { get; set; } = null!;

    public ICollection<InvigilatorShiftGroup> InvigilatorShiftGroups { get; set; } = new List<InvigilatorShiftGroup>();
}