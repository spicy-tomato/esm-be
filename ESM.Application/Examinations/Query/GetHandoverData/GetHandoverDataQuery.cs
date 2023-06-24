using AutoMapper;
using AutoMapper.QueryableExtensions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ESM.Application.Examinations.Query.GetHandoverData;

public record GetHandoverDataQuery(string Id) : IRequest<Result<List<HandoverDataDto>>>;

public class GetHandoverDataQueryHandler
    : IRequestHandler<GetHandoverDataQuery, Result<List<HandoverDataDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IExaminationService _examinationService;

    public GetHandoverDataQueryHandler(IApplicationDbContext context,
        IMapper mapper,
        IExaminationService examinationService)
    {
        _context = context;
        _mapper = mapper;
        _examinationService = examinationService;
    }

    public async Task<Result<List<HandoverDataDto>>> Handle(GetHandoverDataQuery request,
        CancellationToken cancellationToken)
    {
        var examinationGuid = _examinationService.CheckIfExaminationExistAndReturnGuid(request.Id);

        var data = await _context.Shifts
           .Include(s => s.InvigilatorShift)
           .ThenInclude(i => i.Invigilator)
           .Include(s => s.Room)
           .Include(s => s.ShiftGroup)
           .ThenInclude(g => g.Module)
           .Include(s => s.ShiftGroup)
           .Where(g => g.ShiftGroup.ExaminationId == examinationGuid && !g.ShiftGroup.DepartmentAssign)
           .OrderBy(g => g.ShiftGroup.StartAt)
           .ThenBy(g => g.ShiftGroup.ModuleId)
           .ThenBy(g => g.RoomId)
           .ProjectTo<HandoverDataDto>(_mapper.ConfigurationProvider)
           .ToListAsync(cancellationToken);

        return Result<List<HandoverDataDto>>.Get(data);
    }
}