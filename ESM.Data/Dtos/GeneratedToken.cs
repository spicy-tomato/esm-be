using JetBrains.Annotations;

namespace ESM.Data.Dtos;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class GeneratedToken
{
    public string Token { get; set; } = default!;
    public DateTime Expiration { get; set; }
}