using AutoMapper;
using AutoMapper.QueryableExtensions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ESM.Application.Examinations.Queries.GetAllShiftsDetails;

public record GetAllShiftsDetailsQuery(string Id) : IRequest<Result<List<ShiftDetailsDto>>>;

public class GetAllShiftsDetailsQueryHandler : IRequestHandler<GetAllShiftsDetailsQuery, Result<List<ShiftDetailsDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IExaminationService _examinationService;

    public GetAllShiftsDetailsQueryHandler(IApplicationDbContext context,
        IMapper mapper,
        IExaminationService examinationService)
    {
        _context = context;
        _mapper = mapper;
        _examinationService = examinationService;
    }

    public async Task<Result<List<ShiftDetailsDto>>> Handle(GetAllShiftsDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var guid = _examinationService.CheckIfExaminationExistAndReturnGuid(request.Id);

        // @formatter:off
        
        var data = await _context.Shifts
           .Include(s => s.InvigilatorShift)
               .ThenInclude(i => i.Invigilator)
               .ThenInclude(u => u!.Teacher!.Department)
               .ThenInclude(d => d!.Faculty)
           .Include(s => s.Room)
           .Include(s => s.ShiftGroup)
               .ThenInclude(g => g.Module)
           .Where(g => g.ShiftGroup.ExaminationId == guid && !g.ShiftGroup.DepartmentAssign)
           .OrderBy(g => g.ShiftGroup.StartAt)
               .ThenBy(g => g.ShiftGroup.ModuleId)
               .ThenBy(g => g.RoomId)
           .ProjectTo<ShiftDetailsDto>(_mapper.ConfigurationProvider)
           .ToListAsync(cancellationToken);
        
        // @formatter:on

        // @formatter:off
        
        var duplicatedShift = _context.Shifts
           .Select(s => new
            {
                s.RoomId,
                ShiftGroup = new
                {
                    s.ShiftGroup.ExaminationId,
                    s.ShiftGroup.StartAt,
                    s.ShiftGroup.DepartmentAssign,
                    s.ShiftGroup.ModuleId
                }
            })
           .Where(s => s.ShiftGroup.ExaminationId == guid && !s.ShiftGroup.DepartmentAssign)
           .OrderBy(s => s.ShiftGroup.StartAt)
               .ThenBy(s => s.ShiftGroup.ModuleId)
               .ThenBy(s => s.RoomId)
           .GroupBy(s => new 
            {
                 s.ShiftGroup.StartAt, 
                 s.ShiftGroup.ModuleId,
                 s.RoomId 
            })
           .Select(r => new
            {
                r.Key.StartAt,
                r.Key.ModuleId,
                r.Key.RoomId,
                Count = r.Count()
            })
           .Where(r => r.Count > 1)
           .ToList();

        // @formatter:on


        foreach (var shift in data)
        {
            foreach (var ds in duplicatedShift)
            {
                if (ds.StartAt < shift.ShiftGroup.StartAt)
                    continue;
                if (ds.StartAt > shift.ShiftGroup.StartAt)
                    break;
                if (shift.Room.Id != ds.RoomId || shift.ShiftGroup.Module.Id != ds.ModuleId)
                    continue;

                shift.IsDuplicated = true;
                break;
            }
        }

        return Result<List<ShiftDetailsDto>>.Get(data);
    }
}