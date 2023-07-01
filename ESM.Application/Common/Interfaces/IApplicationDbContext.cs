using ESM.Domain.Entities;
using ESM.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ESM.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Candidate> Candidates { get; }
    DbSet<Department> Departments { get; }
    DbSet<DepartmentShiftGroup> DepartmentShiftGroups { get; }
    DbSet<Examination> Examinations { get; }
    DbSet<ExaminationData> ExaminationData { get; }
    DbSet<ExaminationEvent> ExaminationEvents { get; }
    DbSet<Faculty> Faculties { get; }
    DbSet<FacultyShiftGroup> FacultyShiftGroups { get; }
    DbSet<InvigilatorShift> InvigilatorShift { get; }
    DbSet<Module> Modules { get; }
    DbSet<ApplicationRole> Roles { get; }
    DbSet<Room> Rooms { get; }
    DbSet<Shift> Shifts { get; }
    DbSet<ShiftGroup> ShiftGroups { get; }
    DbSet<Teacher> Teachers { get; }
    DbSet<ApplicationUser> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}