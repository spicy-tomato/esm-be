using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ESM.Application.Examinations.Query.GetEvents;

public record GetEventsQuery(string Id) : IRequest<Result<List<ExaminationEvent>>>;

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, Result<List<ExaminationEvent>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IExaminationService _examinationService;

    public GetEventsQueryHandler(IApplicationDbContext context, IExaminationService examinationService)
    {
        _context = context;
        _examinationService = examinationService;
    }

    public async Task<Result<List<ExaminationEvent>>> Handle(GetEventsQuery request,
        CancellationToken cancellationToken)
    {
        var guid = _examinationService.CheckIfExaminationExistAndReturnGuid(request.Id);

        var data = await _context.ExaminationEvents
           .Where(e => e.ExaminationId == guid)
           .ToListAsync(cancellationToken);

        return Result<List<ExaminationEvent>>.Get(data);
    }
}