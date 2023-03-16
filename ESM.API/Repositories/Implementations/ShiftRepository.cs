using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Models;

namespace ESM.API.Repositories.Implementations;

public class ShiftRepository : RepositoryBase<Shift>, IShiftRepository
{
    #region Constructor

    public ShiftRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion
}