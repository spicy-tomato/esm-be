using AutoMapper;
using AutoMapper.QueryableExtensions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ESM.Application.Examinations.Queries.GetAllGroups;

public record GetAllGroupsQuery(string Id) : IRequest<Result<List<GetAllGroupsDto>>>;

public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, Result<List<GetAllGroupsDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IExaminationService _examinationService;

    public GetAllGroupsQueryHandler(IApplicationDbContext context, IMapper mapper,
        IExaminationService examinationService)
    {
        _context = context;
        _mapper = mapper;
        _examinationService = examinationService;
    }

    public async Task<Result<List<GetAllGroupsDto>>> Handle(GetAllGroupsQuery request,
        CancellationToken cancellationToken)
    {
        var guid = _examinationService.CheckIfExaminationExistAndReturnGuid(request.Id,
            ExaminationStatus.AssignFaculty | ExaminationStatus.AssignInvigilator | ExaminationStatus.Closed);

        var data = await _context.ShiftGroups
            .Include(eg => eg.FacultyShiftGroups)
            .Where(e => e.ExaminationId == guid && !e.DepartmentAssign)
            .OrderBy(eg => eg.StartAt)
            .ProjectTo<GetAllGroupsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var facultyShiftGroup = _context.FacultyShiftGroups
            .Where(fg =>
                fg.ShiftGroup.ExaminationId == guid &&
                !fg.ShiftGroup.DepartmentAssign);

        var invigilatorsNumberInFaculties = CountByFaculties();

        foreach (var group in data)
        {
            _examinationService.CalculateInvigilatorsNumberInShift(group,
                facultyShiftGroup.Where(fg => fg.ShiftGroupId == group.Id).ToList(),
                invigilatorsNumberInFaculties);
        }

        return Result<List<GetAllGroupsDto>>.Get(data);
    }

    private Dictionary<Guid, int> CountByFaculties()
    {
        // @formatter:off
        var teachersInFaculty = _context.Teachers
            .Include(u => u.Department)
                .ThenInclude(d => d!.Faculty)
            .Where(u => u.Department != null && u.Department.Faculty != null)
            .GroupBy(u => u.Department!.Faculty!.Id)
            .Select(g => new { id = g.Key, count = g.Count() })
            .ToDictionary(g => g.id, g => g.count);        
        // @formatter:on

        return teachersInFaculty;
    }
}