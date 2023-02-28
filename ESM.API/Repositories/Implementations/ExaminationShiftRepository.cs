using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Models;

namespace ESM.API.Repositories.Implementations;

public class ExaminationShiftRepository : RepositoryBase<ExaminationShift>, IExaminationShiftRepository
{
    #region Constructor

    public ExaminationShiftRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion
}