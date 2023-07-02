using ESM.Application.Common.Exceptions.Core;
using ESM.Domain.Enums;

namespace ESM.Application.Examinations.Exceptions;

public class ChangedInvalidExaminationStatusException : BadRequestException
{
    public ChangedInvalidExaminationStatusException(ExaminationStatus newStatus, ExaminationStatus currentStatus)
        : base($"Cannot change status to {newStatus} (current: {currentStatus})") { }

    public ChangedInvalidExaminationStatusException(IEnumerable<ExaminationStatus> expectedStatuses)
        : base($"Examination status should be {string.Join(", ", expectedStatuses)}") { }

    public ChangedInvalidExaminationStatusException(ExaminationStatus expectedStatuses)
        : this(new List<ExaminationStatus> { expectedStatuses }) { }
}