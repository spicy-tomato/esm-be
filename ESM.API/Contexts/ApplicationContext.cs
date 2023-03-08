using EntityFramework.Exceptions.MySQL.Pomelo;
using ESM.Data.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ESM.API.Contexts;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ApplicationContext : IdentityUserContext<User, Guid>
{
    public override DbSet<User> Users { get; set; } = null!;
    public DbSet<Candidate> Candidates { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<ExaminationData> ExaminationData { get; set; } = null!;
    public DbSet<Examination> Examinations { get; set; } = null!;
    public DbSet<Shift> Shifts { get; set; } = null!;
    public DbSet<ShiftGroup> ShiftGroups { get; set; } = null!;
    public DbSet<Faculty> Faculties { get; set; } = null!;
    public DbSet<Module> Modules { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Seeding the User to Roles table
        builder.Entity<Role>().HasData(
            new Role
            {
                Id = new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                Name = "ExaminationDepartmentHead",
                NormalizedName = "ExaminationDepartmentHead".ToUpper()
            },
            new Role
            {
                Id = new Guid("08db1e1a-7953-4790-8ebe-272e34a8fe18"),
                Name = "Teacher",
                NormalizedName = "Teacher".ToUpper()
            });

        // Seeding the User to AspNetUsers table
        builder.Entity<User>().HasData(
            new User
            {
                Id = new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                UserName = "admin",
                FullName = "Admin",
                NormalizedUserName = "ADMIN",
                PasswordHash = new PasswordHasher<User>().HashPassword(null!, "e10adc3949ba59abbe56e057f20f883e"),
                RoleId = new Guid("08db1e18-c46f-4e76-8e77-69430f54d796")
            }
        );
    }
}