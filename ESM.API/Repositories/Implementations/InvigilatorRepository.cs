using System.Linq.Expressions;
using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ESM.API.Repositories.Implementations;

public class InvigilatorRepository : RepositoryBase<Invigilator>, IInvigilatorRepository
{
    #region Constructor

    public InvigilatorRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion

    public new IEnumerable<Invigilator> Find(Expression<Func<Invigilator, bool>> expression) =>
        Context.Invigilators
           .Include(i => i.User)
           .ThenInclude(u => u.Department)
           .Where(expression);
}