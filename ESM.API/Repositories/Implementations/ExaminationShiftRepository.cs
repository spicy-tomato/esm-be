using System.Linq.Expressions;
using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Dtos.Examination;
using ESM.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ESM.API.Repositories.Implementations;

public class ExaminationShiftRepository : RepositoryBase<ExaminationShift>, IExaminationShiftRepository
{
    #region Constructor

    public ExaminationShiftRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion

    public new IEnumerable<ExaminationShiftSimple> Find(Expression<Func<ExaminationShift, bool>> expression)
    {
        return Mapper.ProjectTo<ExaminationShiftSimple>(Context.ExaminationShifts
           .Include(s => s.Room)
           .Include(s => s.ExaminationShiftGroup)
           .ThenInclude(g => g.Module)
           .Where(expression)
        );
    }
}