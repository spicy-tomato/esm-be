using ESM.Data.Dtos.Department;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace ESM.Data.Dtos.User;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UserSummary
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsMale { get; set; }
    public DateTime CreatedAt { get; set; }
    public DepartmentSummary? Department { get; set; }
    public ICollection<IdentityRole<Guid>>? Roles { get; set; } = new List<IdentityRole<Guid>>();
}