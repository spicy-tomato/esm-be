using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ESM.API.Repositories.Implementations;

public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
{
    #region Constructor

    public DepartmentRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion

    public new Department Create(Department entity, bool saveChanges = true)
    {
        Context.Departments.Include(d => d.Faculty);
        Context.Departments.Add(entity);
        return base.Create(entity);
    }
}