using ESM.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ESM.Domain.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public Teacher? Teacher { get; set; }
}