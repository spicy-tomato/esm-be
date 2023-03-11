using System.Linq.Expressions;
using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Dtos.Examination;
using ESM.Data.Models;

namespace ESM.API.Repositories.Implementations;

public class DepartmentShiftGroupRepository : RepositoryBase<DepartmentShiftGroup>, IDepartmentShiftGroupRepository
{
    #region Constructor

    public DepartmentShiftGroupRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion

    public new List<DepartmentShiftGroupSimple> Find(Expression<Func<DepartmentShiftGroup, bool>> expression)
    {
        return
            Mapper.ProjectTo<DepartmentShiftGroupSimple>(
                Context.DepartmentShiftGroups
                   .Where(expression)
                   .OrderBy(eg => eg.FacultyShiftGroup.ShiftGroup.StartAt)
            ).ToList();
    }
}