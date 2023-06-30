using JetBrains.Annotations;

namespace ESM.Data.Request.User;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UpdateUserRequest
{
    public string Email { get; set; } = null!;
    public string? TeacherId { get; set; }
    public string FullName { get; set; } = null!;
    public bool IsMale { get; set; }
    public string? DepartmentId { get; set; }
}