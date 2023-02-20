using System.Linq.Expressions;
using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Dtos.Faculty;
using ESM.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ESM.API.Repositories.Implementations;

public class FacultyRepository : RepositoryBase<Faculty>, IFacultyRepository
{
    #region Constructor

    public FacultyRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion

    #region Public methods
    
    public IEnumerable<FacultyWithDepartments> GetAllWithDepartments()
    {
        return Mapper.ProjectTo<FacultyWithDepartments>(Context.Faculties.
            Include(f => f.Departments)
        );
    }

    #endregion
}