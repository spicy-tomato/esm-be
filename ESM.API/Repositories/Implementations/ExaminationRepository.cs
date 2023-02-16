using System.Linq.Expressions;
using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Dtos.Examination;
using ESM.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ESM.API.Repositories.Implementations;

public class ExaminationRepository : RepositoryBase<Examination>, IExaminationRepository
{
    #region Constructor

    public ExaminationRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion

    public new ExaminationSummary? GetById(Guid id)
    {
        return Mapper.ProjectTo<ExaminationSummary>(Context.Examinations
           .Include(e => e.CreatedBy)
           .AsSplitQuery()
        ).FirstOrDefault(e => e.Id == id);
    }

    public new IEnumerable<ExaminationSummary> Find(Expression<Func<Examination, bool>> expression)
    {
        return Mapper.ProjectTo<ExaminationSummary>(Context.Examinations
           .Include(e => e.CreatedBy)
           .AsSplitQuery()
           .Where(expression)
        );
    }
}