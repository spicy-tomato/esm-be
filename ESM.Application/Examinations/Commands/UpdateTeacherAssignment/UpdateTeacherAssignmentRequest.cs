using JetBrains.Annotations;

namespace ESM.Application.Examinations.Commands.UpdateTeacherAssignment;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UpdateTeacherAssignmentDto
{
    public string? DepartmentId { get; set; }
    public string? UserId { get; set; }
    public string? TemporaryInvigilatorName { get; set; }
}

[Serializable]
public class UpdateTeacherAssignmentRequest : Dictionary<string, UpdateTeacherAssignmentDto>
{
}