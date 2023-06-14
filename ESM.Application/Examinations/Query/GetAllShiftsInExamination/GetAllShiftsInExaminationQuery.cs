using AutoMapper;
using AutoMapper.QueryableExtensions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ESM.Application.Examinations.Query.GetAllShiftsInExamination;

public record GetAllShiftsInExaminationQuery(string Id) : IRequest<Result<List<ShiftInExaminationDto>>>;

public class GetAllShiftsInExaminationQueryHandler
    : IRequestHandler<GetAllShiftsInExaminationQuery, Result<List<ShiftInExaminationDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IExaminationService _examinationService;

    public GetAllShiftsInExaminationQueryHandler(IApplicationDbContext context, IMapper mapper, IExaminationService examinationService)
    {
        _context = context;
        _mapper = mapper;
        _examinationService = examinationService;
    }

    public async Task<Result<List<ShiftInExaminationDto>>> Handle(GetAllShiftsInExaminationQuery request,
        CancellationToken cancellationToken)
    {
        var guid = _examinationService.CheckIfExaminationExistAndReturnGuid(request.Id,
            ExaminationStatus.AssignFaculty | ExaminationStatus.AssignInvigilator | ExaminationStatus.Closed);

        var data = await _context.Shifts
           .Where(e =>
                e.ShiftGroup.ExaminationId == guid &&
                !e.ShiftGroup.DepartmentAssign)
           .OrderBy(s => s.ShiftGroup.StartAt)
           .ThenBy(s => s.ShiftGroupId)
           .ThenBy(s => s.ShiftGroup.Module.Name)
           .ThenBy(s => s.Room.DisplayId)
           .ProjectTo<ShiftInExaminationDto>(_mapper.ConfigurationProvider)
           .ToListAsync(cancellationToken);

        return Result<List<ShiftInExaminationDto>>.Get(data);
    }
}