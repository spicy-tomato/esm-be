using ESM.Data.Dtos.Department;
using ESM.Data.Models;
using JetBrains.Annotations;

namespace ESM.Data.Dtos.User;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UserSummary
{
    public Guid Id { get; set; }
    public string DisplayId { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsMale { get; set; }
    public DateTime CreatedAt { get; set; }
    public DepartmentSummary DepartmentSummary { get; set; } = null!;
    public ICollection<Role> Roles { get; set; } = new List<Role>();
}