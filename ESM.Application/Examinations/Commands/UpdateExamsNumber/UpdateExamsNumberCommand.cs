using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Domain.Enums;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ESM.Application.Examinations.Commands.UpdateExamsNumber;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record UpdateExamsNumberCommand
    (string Id, UpdateExamsNumberRequest Request) : IRequest<Result<bool>>;

public class UpdateExamsNumberCommandHandler : IRequestHandler<UpdateExamsNumberCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IExaminationService _examinationService;

    public UpdateExamsNumberCommandHandler(IApplicationDbContext context,
        IExaminationService examinationService)
    {
        _context = context;
        _examinationService = examinationService;
    }

    public async Task<Result<bool>> Handle(UpdateExamsNumberCommand request,
        CancellationToken cancellationToken)
    {
        var entity =
            _examinationService.CheckIfExaminationExistAndReturnEntity(request.Id, ExaminationStatus.AssignFaculty);

        const string notFoundMessage = "Shift ID does not exists: ";

        await _context.Entry(entity)
            .Collection(e => e.ShiftGroups)
            .Query()
            .Include(eg => eg.Shifts)
            .LoadAsync(cancellationToken);

        foreach (var (shiftId, examsCount) in request.Request)
        {
            if (!Guid.TryParse(shiftId, out var shiftGuid))
                throw new NotFoundException(notFoundMessage + shiftId);

            var found = false;
            foreach (var shiftGroup in entity.ShiftGroups)
            {
                if (shiftGroup.DepartmentAssign) continue;

                foreach (var shift in shiftGroup.Shifts)
                {
                    if (shift.Id != shiftGuid)
                        continue;

                    shift.ExamsCount = examsCount;
                    found = true;
                    break;
                }
            }

            if (!found)
                throw new NotFoundException(notFoundMessage + shiftId);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Get(true);
    }
}