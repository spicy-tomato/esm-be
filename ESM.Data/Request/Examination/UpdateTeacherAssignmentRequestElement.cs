using JetBrains.Annotations;

namespace ESM.Data.Request.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UpdateTeacherAssignmentRequestElement
{
    public string? DepartmentId { get; set; }
    public string? UserId { get; set; }
    public string? TemporaryInvigilatorName { get; set; }
}