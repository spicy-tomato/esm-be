using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace ESM.Domain.Entities;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class User : IdentityUser<Guid>
{
    public string FullName { get; set; } = null!;

    public bool IsMale { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    public ICollection<Examination> Examinations { get; set; } = new List<Examination>();

    public ICollection<InvigilatorShift> InvigilatorShifts { get; set; } = new List<InvigilatorShift>();

    public ICollection<Shift> HandedOverShifts { get; set; } = new List<Shift>();

    public Role Role { get; set; } = null!;
    public Guid RoleId { get; set; }

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    // For faculty account
    public Guid? FacultyId { get; set; }
    public Faculty? Faculty { get; set; }

    public string? InvigilatorId { get; set; }
}