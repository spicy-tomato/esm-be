using JetBrains.Annotations;

namespace ESM.Data.Request.User;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ChangePasswordRequest
{
    public string Password { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}