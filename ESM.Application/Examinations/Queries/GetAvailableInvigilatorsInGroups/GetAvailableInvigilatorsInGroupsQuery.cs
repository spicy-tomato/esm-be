using AutoMapper;
using AutoMapper.QueryableExtensions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ESM.Application.Examinations.Queries.GetAvailableInvigilatorsInGroups;

public record GetAvailableInvigilatorsInGroupsQuery(string Id) : IRequest<Result<GetAvailableInvigilatorsInGroupsDto>>;

public class GetAvailableInvigilatorsInGroupsQueryHandler
    : IRequestHandler<GetAvailableInvigilatorsInGroupsQuery, Result<GetAvailableInvigilatorsInGroupsDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IExaminationService _examinationService;

    public GetAvailableInvigilatorsInGroupsQueryHandler(IApplicationDbContext context, IMapper mapper,
        IExaminationService examinationService)
    {
        _context = context;
        _mapper = mapper;
        _examinationService = examinationService;
    }

    public Task<Result<GetAvailableInvigilatorsInGroupsDto>> Handle(
        GetAvailableInvigilatorsInGroupsQuery request,
        CancellationToken cancellationToken)
    {
        var guid = _examinationService.CheckIfExaminationExistAndReturnGuid(request.Id);

        // @formatter:off
        var shiftGroups = _context.ShiftGroups
            .Where(s => s.ExaminationId == guid)
            .Include(g => g.FacultyShiftGroups)
                .ThenInclude(fg => fg.DepartmentShiftGroups)
                .ThenInclude(dg => dg.User)
            .ProjectTo<GetAvailableInvigilatorsInGroupsItem>(_mapper.ConfigurationProvider)
            .ToList();
        // @formatter:on

        var priorityFacultyOfShiftGroupsQuery = _context.ShiftGroups
            .Where(g => g.ExaminationId == guid)
            .Include(g => g.Module)
            .Select(g => new { g.Id, g.Module.FacultyId })
            .ToDictionary(g => g.Id, g => g.FacultyId);

        var result = new GetAvailableInvigilatorsInGroupsDto();

        foreach (var group in shiftGroups)
        {
            var list = new List<GetAvailableInvigilatorsInGroupsItem.ResponseItem>();
            var groupId = group.Id.ToString();

            foreach (var facultyShiftGroup in group.FacultyShiftGroups)
            {
                var priorityFacultyId = priorityFacultyOfShiftGroupsQuery[group.Id];
                foreach (var departmentShiftGroup in facultyShiftGroup.DepartmentShiftGroups)
                {
                    if (departmentShiftGroup.User == null &&
                        departmentShiftGroup.TemporaryInvigilatorName.IsNullOrEmpty())
                        continue;

                    var isPriority = facultyShiftGroup.FacultyId == priorityFacultyId;
                    GetAvailableInvigilatorsInGroupsItem.ResponseItem item = departmentShiftGroup.User == null
                        ? new GetAvailableInvigilatorsInGroupsItem.TemporaryInvigilator
                        {
                            TemporaryName = departmentShiftGroup.TemporaryInvigilatorName!,
                            DepartmentId = departmentShiftGroup.DepartmentId,
                            IsPriority = isPriority
                        }
                        : new GetAvailableInvigilatorsInGroupsItem.VerifiedInvigilator
                        {
                            Id = departmentShiftGroup.User.Id,
                            FullName = departmentShiftGroup.User.FullName,
                            InvigilatorId = departmentShiftGroup.User.InvigilatorId,
                            IsPriority = isPriority,
                            PhoneNumber = departmentShiftGroup.User.PhoneNumber,
                            FacultyName = departmentShiftGroup.User.Department?.Faculty?.Name,
                            DepartmentName = departmentShiftGroup.User.Department?.Name
                        };

                    list.Add(item);
                }
            }

            result.Add(groupId, list);
        }

        return Task.FromResult(Result<GetAvailableInvigilatorsInGroupsDto>.Get(result));
    }
}