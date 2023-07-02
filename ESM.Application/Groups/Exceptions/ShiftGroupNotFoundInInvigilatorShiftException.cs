using ESM.Application.Common.Exceptions.Core;

namespace ESM.Application.Groups.Exceptions;

public class ShiftGroupNotFoundInInvigilatorShiftException : BadRequestException
{
    public ShiftGroupNotFoundInInvigilatorShiftException(string shiftGroupId, Guid invigilatorShiftId) : base(
        $"Shift group ID does not exist: {shiftGroupId} (in invigilatorShift ID: {invigilatorShiftId}") { }
}