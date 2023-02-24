using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class User : IdentityUser<Guid>
{
    public string FullName { get; set; } = null!;

    public bool IsMale { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    public ICollection<IdentityRole<Guid>> Roles { get; set; } = new List<IdentityRole<Guid>>();

    public ICollection<Examination> Examinations { get; set; } = new List<Examination>();

    public ICollection<InvigilatorShift> CreatorInvigilatorShift { get; set; } = new List<InvigilatorShift>();

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public Guid? InvigilatorId { get; set; }
    public Invigilator? Invigilator { get; set; }
}