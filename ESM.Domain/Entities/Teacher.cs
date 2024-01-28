using System.ComponentModel.DataAnnotations;
using ESM.Domain.Common;
using ESM.Domain.Identity;
using JetBrains.Annotations;

namespace ESM.Domain.Entities;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Teacher : BaseAuditableEntity
{
    [MaxLength(20)]
    public string? TeacherId { get; set; }

    [MaxLength(100)]
    public string FullName { get; set; } = null!;

    public bool IsMale { get; set; }

    public ICollection<Examination> Examinations { get; set; } = new List<Examination>();

    public ICollection<InvigilatorShift> InvigilatorShifts { get; set; } = new List<InvigilatorShift>();

    public ICollection<Shift> HandedOverShifts { get; set; } = new List<Shift>();

    public ApplicationUser User { get; set; } = null!;
    public Guid UserId { get; set; }

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    // For faculty account
    public Guid? FacultyId { get; set; }
    public Faculty? Faculty { get; set; }
}