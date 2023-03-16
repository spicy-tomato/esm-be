using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Models;

namespace ESM.API.Repositories.Implementations;

public class DepartmentShiftGroupRepository : RepositoryBase<DepartmentShiftGroup>, IDepartmentShiftGroupRepository
{
    #region Constructor

    public DepartmentShiftGroupRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion
}