using ESM.Application.Common.Exceptions.Core;

namespace ESM.Application.Shifts.Exceptions;

public class UserNotAssignToShiftException : BadRequestException
{
    public UserNotAssignToShiftException() : base("User ID is not assigned in this shift") { }
}