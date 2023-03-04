using System.Linq.Expressions;
using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Dtos.Examination;
using ESM.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ESM.API.Repositories.Implementations;

public class ExaminationShiftGroupRepository : RepositoryBase<ExaminationShiftGroup>, IExaminationShiftGroupRepository
{
    #region Constructor

    public ExaminationShiftGroupRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion

    public new IEnumerable<ExaminationShiftGroupSimple> Find(Expression<Func<ExaminationShiftGroup, bool>> expression)
    {
        return Mapper.ProjectTo<ExaminationShiftGroupSimple>(
            Context.ExaminationShiftGroups
               .Include(eg => eg.FacultyExaminationShiftGroups)
               .Where(expression)
               .OrderBy(eg => eg.StartAt)
        );
    }

    public new ExaminationShiftGroupSimple? FindOne(Expression<Func<ExaminationShiftGroup, bool>> expression)
    {
        return Mapper.ProjectTo<ExaminationShiftGroupSimple>(
            Context.ExaminationShiftGroups
               .Include(eg => eg.FacultyExaminationShiftGroups)
               .Where(expression)
        ).FirstOrDefault();
    }
}