using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Models;

namespace ESM.API.Repositories.Implementations;

public class FacultyShiftGroupRepository : RepositoryBase<FacultyShiftGroup>, IFacultyShiftGroupRepository
{
    #region Constructor

    public FacultyShiftGroupRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion

    // public new List<FacultyShiftGroupSimple> Find(Expression<Func<FacultyShiftGroup, bool>> expression)
    // {
    //     return Mapper.ProjectTo<FacultyShiftGroupSimple>(
    //         Context.FacultyShiftGroups
    //            .Where(expression)
    //            .OrderBy(eg => eg.ShiftGroup.StartAt)
    //     ).ToList();
    // }
}