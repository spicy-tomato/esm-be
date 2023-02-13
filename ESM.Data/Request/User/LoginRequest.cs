using JetBrains.Annotations;

namespace ESM.Data.Request.User;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class LoginRequest
{
    // Can be email
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
}