using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Application.Shifts.Exceptions;
using ESM.Application.Users.Exceptions;
using ESM.Domain.Entities;
using MediatR;

namespace ESM.Application.Shifts.Commands.Update;

public record UpdateCommand(string Id, UpdateRequest Request) : IRequest<Result<bool>>;

public class UpdateCommandHandler : IRequestHandler<UpdateCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IShiftService _shiftService;
    private readonly IIdentityService _identityService;

    public UpdateCommandHandler(IApplicationDbContext context,
        IShiftService shiftService,
        IIdentityService identityService)
    {
        _context = context;
        _shiftService = shiftService;
        _identityService = identityService;
    }

    public async Task<Result<bool>> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var shift = _shiftService.CheckIfShiftExistAndReturnEntity(request.Id);

        await UpdateHandoverTeacher(shift, request.Request.HandoverTeacherId);
        UpdateReport(shift, request.Request.Report);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Get(true);
    }

    private async Task UpdateHandoverTeacher(Shift shift, string? handoverTeacherId)
    {
        if (handoverTeacherId == null)
        {
            return;
        }

        if (await _identityService.FindUserByIdAsync(handoverTeacherId) is null)
        {
            throw new UserNotFoundException(handoverTeacherId);
        }

        var teacherGuid = Guid.Parse(handoverTeacherId);

        var teacherIdIsValid = shift.InvigilatorShift.Any(ivs => ivs.InvigilatorId == teacherGuid);
        if (!teacherIdIsValid)
        {
            throw new UserNotAssignToShiftException();
        }

        shift.HandedOverUserId = teacherGuid;
    }

    private static void UpdateReport(Shift shift, string? report)
    {
        if (report != null)
        {
            shift.Report = report;
        }
    }
}