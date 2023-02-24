using System.Linq.Expressions;
using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Dtos.User;
using ESM.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ESM.API.Repositories.Implementations;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    #region Constructor

    public UserRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion

    public new UserSummary? GetById(Guid id)
    {
        return Mapper.ProjectTo<UserSummary>(
            Context.Users
               .Include(u => u.Department)
               .Include(u => u.Roles)
               .AsSplitQuery()
               .Where(u => u.Id == id)
        ).FirstOrDefault();
    }

    public new IEnumerable<UserSimple> Find(Expression<Func<User, bool>> expression) =>
        Mapper.ProjectTo<UserSimple>(Context.Users.Include(u => u.Invigilator).Where(expression));
}