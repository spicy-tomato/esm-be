using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Models;

namespace ESM.API.Repositories.Implementations;

public class ExaminationDataRepository : RepositoryBase<ExaminationData>, IExaminationDataRepository
{
    #region Constructor

    public ExaminationDataRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion

    public new void CreateRange(IEnumerable<ExaminationData> entities)
    {
        base.CreateRange(entities);
        Context.SaveChanges();
    }
}