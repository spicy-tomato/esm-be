using EntityFramework.Exceptions.MySQL.Pomelo;
using ESM.Data.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ESM.API.Contexts;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ApplicationContext : IdentityUserContext<User, Guid>
{
    public DbSet<School> Schools { get; set; } = null!;
    public DbSet<Faculty> Faculties { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<Right> Rights { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<TemporaryRight> TemporaryRights { get; set; } = null!;
    public override DbSet<User> Users { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
    }
}