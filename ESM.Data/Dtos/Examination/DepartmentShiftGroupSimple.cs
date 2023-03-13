using ESM.Data.Dtos.Department;
using ESM.Data.Dtos.User;
using JetBrains.Annotations;

namespace ESM.Data.Dtos.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class DepartmentShiftGroupSimple
{
    public Guid Id { get; set; }
    public Guid? DepartmentId { get; set; }
    public UserSimple? User { get; set; }
    public string? TemporaryInvigilatorName { get; set; }
    public FacultyShiftGroupInDepartmentShiftGroupSimple FacultyShiftGroup { get; set; } = null!;
}