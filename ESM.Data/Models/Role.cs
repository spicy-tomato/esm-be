using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Role : IdentityRole<Guid>
{
    public ICollection<User> Users { get; set; } = new List<User>();
}