using ESM.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public Teacher? Teacher { get; set; }
    public Guid? TeacherId { get; set; }
}