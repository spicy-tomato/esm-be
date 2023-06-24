using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Data.Models;
using ESM.Domain.Entities;
using JetBrains.Annotations;
using MediatR;

namespace ESM.Application.Shifts.Commands.Update;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record UpdateCommand(string Id, UpdateRequest Request) : IRequest<Result<bool>>;

public class UpdateCommandHandler : IRequestHandler<UpdateCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IShiftService _shiftService;
    private readonly IUserService _userService;

    public UpdateCommandHandler(IApplicationDbContext context, IShiftService shiftService, IUserService userService)
    {
        _context = context;
        _shiftService = shiftService;
        _userService = userService;
    }

    public async Task<Result<bool>> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var shift = _shiftService.CheckIfShiftExistAndReturnEntity(request.Id);

        UpdateHandoverTeacher(shift, request.Request.HandoverTeacherId);
        UpdateReport(shift, request.Request.Report);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Get(true);
    }

    private void UpdateHandoverTeacher(Shift shift, string? handoverTeacherId)
    {
        if (handoverTeacherId == null)
        {
            return;
        }

        if (!_userService.UserExist(handoverTeacherId))
        {
            throw new NotFoundException(nameof(Teacher), handoverTeacherId);
        }

        var teacherGuid = Guid.Parse(handoverTeacherId);

        var teacherIdIsValid = shift.InvigilatorShift.Any(ivs => ivs.InvigilatorId == teacherGuid);
        if (!teacherIdIsValid)
        {
            throw new BadRequestException("User ID is not assigned in this shift");
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