using JetBrains.Annotations;

namespace ESM.Data.Dtos.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class FacultyShiftGroupInDepartmentShiftGroupSimple
{
    public Guid Id { get; set; }
    public ShiftGroupInDepartmentShiftGroupSimple ShiftGroup { get; set; } = null!;
}