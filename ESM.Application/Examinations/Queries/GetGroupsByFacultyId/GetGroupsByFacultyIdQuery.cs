using AutoMapper;
using AutoMapper.QueryableExtensions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ESM.Application.Examinations.Queries.GetGroupsByFacultyId;

public record GetGroupsByFacultyIdQuery
    (string Id, string FacultyId) : IRequest<Result<List<GetGroupsByFacultyIdDto>>>;

public class GetGroupsByFacultyIdQueryHandler
    : IRequestHandler<GetGroupsByFacultyIdQuery, Result<List<GetGroupsByFacultyIdDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IExaminationService _examinationService;
    private readonly IFacultyService _facultyService;

    public GetGroupsByFacultyIdQueryHandler(IApplicationDbContext context, IMapper mapper,
        IExaminationService examinationService, IFacultyService facultyService)
    {
        _context = context;
        _mapper = mapper;
        _examinationService = examinationService;
        _facultyService = facultyService;
    }

    public async Task<Result<List<GetGroupsByFacultyIdDto>>> Handle(GetGroupsByFacultyIdQuery request,
        CancellationToken cancellationToken)
    {
        var examinationGuid = _examinationService.CheckIfExaminationExistAndReturnGuid(request.Id,
            ExaminationStatus.AssignInvigilator | ExaminationStatus.Closed);

        var facultyGuid = _facultyService.CheckIfExistAndReturnGuid(request.FacultyId);

        // @formatter:off
        var data = await _context.DepartmentShiftGroups
            .Where(fg =>
                fg.FacultyShiftGroup.ShiftGroup.ExaminationId == examinationGuid &&
                fg.FacultyShiftGroup.FacultyId == facultyGuid
            )
            .Include(dg => dg.FacultyShiftGroup)
                .ThenInclude(fg => fg.ShiftGroup)
            .OrderBy(eg => eg.FacultyShiftGroup.ShiftGroup.StartAt)
            .ProjectTo<GetGroupsByFacultyIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        // @formatter:on

        return Result<List<GetGroupsByFacultyIdDto>>.Get(data);
    }
}