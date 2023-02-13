using JetBrains.Annotations;

namespace ESM.Data.Request.User;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class CreateUserRequest
{
    public string UserName { get; set; } = null!;
    public string? Email { get; set; }
    public string Password { get; set; } = null!;
    public string? DisplayId { get; set; }
    public string FullName { get; set; } = null!;
}