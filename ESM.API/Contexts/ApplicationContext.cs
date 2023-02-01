using ESM.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ESM.API.Contexts;

public class ApplicationContext : IdentityUserContext<User>
{
    public override DbSet<User> Users { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
}