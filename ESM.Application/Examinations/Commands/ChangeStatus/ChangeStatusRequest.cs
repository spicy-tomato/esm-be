using ESM.Data.Enums;
using JetBrains.Annotations;

namespace ESM.Application.Examinations.Commands.ChangeStatus;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ChangeStatusRequest
{
    public ExaminationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}