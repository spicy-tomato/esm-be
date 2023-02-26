using JetBrains.Annotations;

namespace ESM.Data.Dtos.User;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UserSimple
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsMale { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? InvigilatorId { get; set; }
}