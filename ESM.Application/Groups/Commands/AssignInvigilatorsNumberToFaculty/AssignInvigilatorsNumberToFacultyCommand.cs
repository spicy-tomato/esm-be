using AutoMapper;
using AutoMapper.QueryableExtensions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Application.Examinations.Exceptions;
using ESM.Domain.Dtos.Examination;
using ESM.Domain.Entities;
using ESM.Domain.Interfaces;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ESM.Application.Groups.Commands.AssignInvigilatorsNumberToFaculty;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record AssignInvigilatorsNumberToFacultyCommand(string Id, string FacultyId, int NumberOfInvigilators)
    : IRequest<Result<AssignInvigilatorsNumberToFacultyDto?>>;

public class AssignInvigilatorsNumberToFacultyCommandHandler
    : IRequestHandler<AssignInvigilatorsNumberToFacultyCommand, Result<AssignInvigilatorsNumberToFacultyDto?>>
{
    private readonly IApplicationDbContext _context;
    private readonly IFacultyService _facultyService;
    private readonly IGroupService _groupService;
    private readonly IMapper _mapper;

    public AssignInvigilatorsNumberToFacultyCommandHandler(IApplicationDbContext context, IGroupService groupService,
        IFacultyService facultyService, IMapper mapper)
    {
        _context = context;
        _groupService = groupService;
        _facultyService = facultyService;
        _mapper = mapper;
    }

    public async Task<Result<AssignInvigilatorsNumberToFacultyDto?>> Handle(
        AssignInvigilatorsNumberToFacultyCommand request,
        CancellationToken cancellationToken)
    {
        if (request.NumberOfInvigilators < 0)
        {
            throw new NegativeInvigilatorNumberException();
        }

        var group = _groupService.CheckIfGroupExistAndReturnEntity(request.Id);
        var facultyGuid = _facultyService.CheckIfExistAndReturnGuid(request.Id);

        var facultyGroup = group.FacultyShiftGroups
            .FirstOrDefault(eg => eg.FacultyId == facultyGuid);
        if (facultyGroup == null)
        {
            facultyGroup = new FacultyShiftGroup
            {
                FacultyId = facultyGuid,
                ShiftGroup = group
            };
            group.FacultyShiftGroups.Add(facultyGroup);
        }

        facultyGroup.InvigilatorsCount = request.NumberOfInvigilators;

        await _context.SaveChangesAsync(cancellationToken);

        var data = _context.ShiftGroups
            .Include(eg => eg.FacultyShiftGroups)
            .Where(e => !e.DepartmentAssign && e.Id == group.Id)
            .ProjectTo<AssignInvigilatorsNumberToFacultyDto>(_mapper.ConfigurationProvider)
            .FirstOrDefault();

        var invigilatorsNumberInFaculties = CountByFaculties();

        var facultyShiftGroup = _context.FacultyShiftGroups
            .Where(fg => fg.ShiftGroupId == group.Id && !fg.ShiftGroup.DepartmentAssign)
            .ToList();

        if (data != null)
        {
            CalculateShiftInvigilatorsNumber(data, facultyShiftGroup, invigilatorsNumberInFaculties);
        }

        return Result<AssignInvigilatorsNumberToFacultyDto?>.Get(data);
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

    private static void CalculateShiftInvigilatorsNumber<T>(T group,
        ICollection<FacultyShiftGroup> facultyShiftGroup,
        IReadOnlyDictionary<Guid, int> invigilatorsNumberInFaculties) where T : IShiftGroup
    {
        group.AssignNumerate = facultyShiftGroup
            .ToDictionary(
                fg => fg.FacultyId.ToString(),
                fg => new ShiftGroupDataCell
                {
                    Actual = fg.InvigilatorsCount,
                    Calculated = fg.CalculatedInvigilatorsCount,
                    Maximum = invigilatorsNumberInFaculties.GetValueOrDefault(fg.FacultyId, 0)
                }
            );

        var total = facultyShiftGroup.Sum(feg => feg.InvigilatorsCount);
        group.AssignNumerate.Add("total",
            new ShiftGroupDataCell
            {
                // Actual calculation result
                Actual = total,
                // Difference
                Calculated = total - group.InvigilatorsCount
            });
    }
}