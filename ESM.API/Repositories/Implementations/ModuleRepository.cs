using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Models;

namespace ESM.API.Repositories.Implementations;

public class ModuleRepository : RepositoryBase<Module>, IModuleRepository
{
    #region Constructor

    public ModuleRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion

    public IEnumerable<string> GetIds() => Context.Modules.Select(m => m.DisplayId);
}