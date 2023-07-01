using ESM.Domain.Dtos.Department;
using ESM.Domain.Dtos.Faculty;
using JetBrains.Annotations;

namespace ESM.Domain.Dtos.User;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UserSummary
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string? Email { get; set; }
    public bool IsMale { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? InvigilatorId { get; set; }
    public DepartmentSummary? Department { get; set; }
    public FacultySummary? Faculty { get; set; }
    public string Role { get; set; } = null!;
    public string? PhoneNumber { get; set; }
}