using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Data.Enums;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ESM.Application.Examinations.Commands.AssignInvigilatorsToShifts;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record AssignInvigilatorsToShiftsCommand
    (string Id, AssignInvigilatorsToShiftsRequest Request) : IRequest<Result<bool>>;

public class AssignInvigilatorsToShiftsCommandHandler : IRequestHandler<AssignInvigilatorsToShiftsCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IExaminationService _examinationService;

    public AssignInvigilatorsToShiftsCommandHandler(IApplicationDbContext context,
        IExaminationService examinationService)
    {
        _context = context;
        _examinationService = examinationService;
    }

    public async Task<Result<bool>> Handle(AssignInvigilatorsToShiftsCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Request.IsNullOrEmpty())
            return Result<bool>.Get(true);

        var entity =
            _examinationService.CheckIfExaminationExistAndReturnEntity(request.Id, ExaminationStatus.AssignInvigilator);

        await _context.Entry(entity)
           .Collection(e => e.ShiftGroups)
           .Query()
           .Include(eg => eg.Shifts)
           .ThenInclude(fg => fg.InvigilatorShift)
           .AsSplitQuery()
           .LoadAsync(cancellationToken);

        foreach (var shiftGroup in entity.ShiftGroups)
            foreach (var shift in shiftGroup.Shifts)
                foreach (var invigilatorShift in shift.InvigilatorShift)
                {
                    if (!request.Request.TryGetValue(invigilatorShift.Id.ToString(), out var invigilatorId))
                        continue;

                    if (invigilatorId == null)
                    {
                        invigilatorShift.InvigilatorId = null;
                        request.Request.Remove(invigilatorShift.Id.ToString());
                        continue;
                    }

                    if (!Guid.TryParse(invigilatorId, out var invigilatorGuid))
                        throw new BadRequestException($"Cannot parse invigilator ID to Guid: {invigilatorId}");

                    invigilatorShift.InvigilatorId = invigilatorGuid;
                    request.Request.Remove(invigilatorShift.Id.ToString());
                }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Get(true);
    }
}