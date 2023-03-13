using JetBrains.Annotations;

namespace ESM.Data.Request.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UpdateTeacherAssignmentRequestElement
{
    public string? DepartmentId;
    public string? UserId;
    public string? TemporaryInvigilatorName;
}