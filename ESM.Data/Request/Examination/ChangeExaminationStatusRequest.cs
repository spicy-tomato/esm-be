using ESM.Data.Enums;
using JetBrains.Annotations;

namespace ESM.Data.Request.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ChangeExaminationStatusRequest
{
    public ExaminationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}