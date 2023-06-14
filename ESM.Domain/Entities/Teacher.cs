using ESM.Data.Models;
using ESM.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace ESM.Domain.Entities;

public class Teacher : BaseAuditableEntity
{
    public string? TeacherId { get; set; }

    public string FullName { get; set; } = null!;

    public bool IsMale { get; set; }

    public ICollection<Examination> Examinations { get; set; } = new List<Examination>();

    public ICollection<InvigilatorShift> InvigilatorShifts { get; set; } = new List<InvigilatorShift>();

    public ICollection<Shift> HandedOverShifts { get; set; } = new List<Shift>();

    public IdentityUser<Guid> User { get; set; } = null!;
    public Guid UserId { get; set; }

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    // For faculty account
    public Guid? FacultyId { get; set; }
    public Faculty? Faculty { get; set; }
}