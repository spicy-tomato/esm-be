using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESM.Domain.Identity;
using JetBrains.Annotations;

namespace ESM.Domain.Entities;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class DepartmentShiftGroup
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public Guid? UserId { get; set; }
    public ApplicationUser? User { get; set; }
    
    public string? TemporaryInvigilatorName { get; set; }

    public Guid FacultyShiftGroupId { get; set; }
    public FacultyShiftGroup FacultyShiftGroup { get; set; } = null!;
}