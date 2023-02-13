using ESM.Data.Dtos.School;
using JetBrains.Annotations;

namespace ESM.Data.Dtos.User;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class SimpleUser
{
    public Guid Id { get; set; }
    public string DisplayId { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public SimpleSchool School { get; set; } = null!;
}