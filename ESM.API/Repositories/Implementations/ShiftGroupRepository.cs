using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Models;

namespace ESM.API.Repositories.Implementations;

public class ShiftGroupRepository : RepositoryBase<ShiftGroup>, IShiftGroupRepository
{
    #region Constructor

    public ShiftGroupRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion
}