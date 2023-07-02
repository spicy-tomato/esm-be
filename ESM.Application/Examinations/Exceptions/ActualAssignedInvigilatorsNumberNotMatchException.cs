using ESM.Application.Common.Exceptions.Core;
using ESM.Domain.Entities;

namespace ESM.Application.Examinations.Exceptions;

public class ActualAssignedInvigilatorsNumberNotMatchException : BadRequestException
{
    public ActualAssignedInvigilatorsNumberNotMatchException(int actual, int expected, ShiftGroup group)
        : base(
            $"Actual assigned invigilators number is not match (met {actual}, need {expected}) in group {group.Id} ({group.Module.Name})") { }
}