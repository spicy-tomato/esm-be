using System.Linq.Expressions;
using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Common.Core;
using ESM.Data.Dtos.User;
using ESM.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ESM.API.Repositories.Implementations;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    #region Constructor

    public UserRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion

    public new IEnumerable<UserSummary> GetAll() => Mapper.ProjectTo<UserSummary>(Context.Users);

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

    public new IEnumerable<UserSummary> Find(Expression<Func<User, bool>> expression) =>
        Mapper.ProjectTo<UserSummary>(Context.Users.Where(expression));

    public List<Error> GetDuplicatedDataErrors(Guid id, string? email, string? invigilatorId)
    {
        var errorList = new List<Error>();
        var usersWithDuplicatedData = Find(
            u => u.Id != id &&
                 ((u.Email != null && u.Email == email) ||
                  (u.InvigilatorId != null && u.InvigilatorId != invigilatorId))
        );

        foreach (var u in usersWithDuplicatedData)
        {
            if (u.Email != null && u.Email == email)
                errorList.Add(new Error("email", "Email"));
            if (u.InvigilatorId != null && u.InvigilatorId == invigilatorId)
                errorList.Add(new Error("invigilatorId", "Mã CBCT"));
        }

        return errorList;
    }
}