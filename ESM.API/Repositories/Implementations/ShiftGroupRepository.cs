using System.Linq.Expressions;
using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Dtos.Examination;
using ESM.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ESM.API.Repositories.Implementations;

public class ShiftGroupRepository : RepositoryBase<ShiftGroup>, IShiftGroupRepository
{
    #region Constructor

    public ShiftGroupRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion

    public new IEnumerable<ShiftGroupSimple> Find(Expression<Func<ShiftGroup, bool>> expression)
    {
        return Mapper.ProjectTo<ShiftGroupSimple>(
            Context.ShiftGroups
               .Include(eg => eg.FacultyShiftGroups)
               .Where(expression)
               .OrderBy(eg => eg.StartAt)
        );
    }

    public new ShiftGroupSimple? FindOne(Expression<Func<ShiftGroup, bool>> expression)
    {
        return Mapper.ProjectTo<ShiftGroupSimple>(
            Context.ShiftGroups
               .Include(eg => eg.FacultyShiftGroups)
               .Where(expression)
        ).FirstOrDefault();
    }
}