using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Role : IdentityRole<Guid>
{
    public ICollection<Right> Rights { get; set; } = new List<Right>();
}