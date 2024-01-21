using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Domain.Enums;
using MediatR;

namespace ESM.Application.Examinations.Queries.GetStatistic;

public record GetStatisticQuery(string Id) : IRequest<Result<GetStatisticDto>>;

public class GetStatisticQueryHandler : IRequestHandler<GetStatisticQuery, Result<GetStatisticDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IExaminationService _examinationService;

    public GetStatisticQueryHandler(IApplicationDbContext context, IExaminationService examinationService)
    {
        _context = context;
        _examinationService = examinationService;
    }

    public Task<Result<GetStatisticDto>> Handle(GetStatisticQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.Now;
        var entity = _examinationService.CheckIfExaminationExistAndReturnEntity(request.Id,
            ExaminationStatus.AssignFaculty | ExaminationStatus.AssignInvigilator | ExaminationStatus.Closed);

        var startAt = _context.ShiftGroups
            .Where(g => g.ExaminationId == entity.Id)
            .Min(g => g.StartAt)
            .Date;
        var endAt = _context.ShiftGroups
            .Where(g => g.ExaminationId == entity.Id)
            .Max(g => g.StartAt)
            .AddDays(1).Date;
        var numberOfModules = _context.ShiftGroups
            .Where(g => g.ExaminationId == entity.Id)
            .GroupBy(g => g.ModuleId)
            .Count();
        var numberOfModulesOver = _context.ShiftGroups
            .Where(g => g.ExaminationId == entity.Id)
            .GroupBy(g => new
            {
                g.ModuleId,
                Over = g.StartAt < now
            })
            .Count(g => g.All(x => x.StartAt < now));
        var numberOfShifts = _context.Shifts
            .Count(s => s.ShiftGroup.ExaminationId == entity.Id);
        var numberOfShiftsOver = _context.Shifts
            .Count(s => s.ShiftGroup.StartAt < now &&
                        s.ShiftGroup.ExaminationId == entity.Id);
        var numberOfCandidates = _context.Shifts
            .Where(s => s.ShiftGroup.ExaminationId == entity.Id)
            .Sum(s => s.CandidatesCount);
        var numberOfInvigilators = _context.InvigilatorShift
            .Where(ivs =>
                ivs.DeletedAt == null &&
                ivs.InvigilatorId != null &&
                ivs.Shift.ShiftGroup.ExaminationId == entity.Id)
            .GroupBy(ivs => ivs.InvigilatorId)
            .Count();

        var result = new GetStatisticDto
        {
            Id = entity.Id,
            DisplayId = entity.DisplayId,
            Name = entity.Name,
            StartAt = startAt,
            EndAt = endAt,
            TimePercent = endAt < now
                ? 100
                : (now - startAt) * 100f / (endAt - startAt),
            NumberOfModules = numberOfModules,
            NumberOfModulesOver = numberOfModulesOver,
            NumberOfShifts = numberOfShifts,
            NumberOfShiftsOver = numberOfShiftsOver,
            NumberOfCandidates = numberOfCandidates,
            NumberOfInvigilators = numberOfInvigilators
        };

        return Task.FromResult(Result<GetStatisticDto>.Get(result));
    }
}