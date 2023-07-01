using System.Reflection;
using EntityFramework.Exceptions.MySQL.Pomelo;
using ESM.Application.Common.Interfaces;
using ESM.Domain.Entities;
using ESM.Domain.Identity;
using ESM.Infrastructure.Common;
using ESM.Infrastructure.Persistence.Interceptors;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Module = ESM.Domain.Entities.Module;

namespace ESM.Infrastructure.Persistence;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IApplicationDbContext
{
    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<DepartmentShiftGroup> DepartmentShiftGroups => Set<DepartmentShiftGroup>();
    public DbSet<Examination> Examinations => Set<Examination>();
    public DbSet<ExaminationData> ExaminationData => Set<ExaminationData>();
    public DbSet<ExaminationEvent> ExaminationEvents => Set<ExaminationEvent>();
    public DbSet<Faculty> Faculties => Set<Faculty>();
    public DbSet<FacultyShiftGroup> FacultyShiftGroups => Set<FacultyShiftGroup>();
    public DbSet<InvigilatorShift> InvigilatorShift => Set<InvigilatorShift>();
    public DbSet<Module> Modules => Set<Module>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Shift> Shifts => Set<Shift>();
    public DbSet<ShiftGroup> ShiftGroups => Set<ShiftGroup>();
    public DbSet<Teacher> Teachers => Set<Teacher>();

    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);

        RenameAspTables(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        await _mediator.DispatchDomainEvents(this);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private static void RenameAspTables(ModelBuilder builder)
    {
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
        builder.Entity<ApplicationRole>().ToTable("Roles");
        builder.Entity<ApplicationUser>().ToTable("Users");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
    }
}