using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ESM.API.Repositories.Implementations;

public class ModuleRepository : RepositoryBase<Module>, IModuleRepository
{
    #region Constructor

    public ModuleRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion

    public Task<List<string>> GetIdsAsync() => Context.Modules.Select(m => m.DisplayId).ToListAsync();
}