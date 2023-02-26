using JetBrains.Annotations;

namespace ESM.Data.Request.User;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class CreateUserRequest
{
    public string Email { get; set; } = null!;
    public string? InvigilatorId { get; set; }
    public string FullName { get; set; } = null!;
    public bool IsMale { get; set; }
}