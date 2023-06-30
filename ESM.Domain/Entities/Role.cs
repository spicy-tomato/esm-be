using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace ESM.Domain.Entities;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Role : IdentityRole<Guid>
{
    public ICollection<User> Users { get; set; } = new List<User>();
}