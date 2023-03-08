using System.Linq.Expressions;
using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Dtos.Examination;
using ESM.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ESM.API.Repositories.Implementations;

public class ShiftRepository : RepositoryBase<Shift>, IShiftRepository
{
    #region Constructor

    public ShiftRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion

    public new IEnumerable<ShiftSimple> Find(Expression<Func<Shift, bool>> expression)
    {
        return Mapper.ProjectTo<ShiftSimple>(Context.Shifts
           .Include(s => s.Room)
           .Include(s => s.ShiftGroup)
           .ThenInclude(g => g.Module)
           .Where(expression)
        );
    }
}