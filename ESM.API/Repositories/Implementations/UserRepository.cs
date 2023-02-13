using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Models;

namespace ESM.API.Repositories.Implementations;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    #region Constructor

    public UserRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion
}