using System.Linq.Expressions;
using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Dtos.Examination;
using ESM.Data.Enums;
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
        return Mapper.ProjectTo<ExaminationSummary>(Context.Examinations.Include(e => e.CreatedBy))
           .FirstOrDefault(e => e.Id == id);
    }

    public ExaminationStatus? GetStatus(Guid id)
    {
        var examination = Context.Examinations
           .Select(e => new { e.Id, e.Status })
           .FirstOrDefault(u => u.Id == id);
        return examination?.Status;
    }
}