using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ESM.API.Repositories.Implementations;

public class FacultyRepository : RepositoryBase<Faculty>, IFacultyRepository
{
    #region Constructor

    public FacultyRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion
}