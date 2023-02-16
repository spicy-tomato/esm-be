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

    public ICollection<Role> Roles { get; set; } = new List<Role>();

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public ICollection<Examination> Examinations { get; set; } = new List<Examination>();

    public ICollection<InvigilatorExaminationModule> ExaminationModules { get; set; } =
        new List<InvigilatorExaminationModule>();

    [InverseProperty("Invigilator")]
    public ICollection<InvigilatorShift> InvigilatorShift { get; set; } = new List<InvigilatorShift>();

    [InverseProperty("CreatedBy")]
    public ICollection<InvigilatorShift> CreatorInvigilatorShift { get; set; } = new List<InvigilatorShift>();

    [InverseProperty("User")]
    public ICollection<TemporaryRight> TemporaryRights { get; set; } = new List<TemporaryRight>();

    [InverseProperty("GrantedBy")]
    public ICollection<TemporaryRight> GrantedTemporaryRights { get; set; } = new List<TemporaryRight>();
}