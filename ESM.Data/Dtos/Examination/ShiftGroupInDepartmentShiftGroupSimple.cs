using ESM.Data.Dtos.Module;
using ESM.Data.Enums;
using JetBrains.Annotations;

namespace ESM.Data.Dtos.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ShiftGroupInDepartmentShiftGroupSimple
{
    public Guid Id { get; set; }
    public ExamMethod Method { get; set; }
    public DateTime StartAt { get; set; }
    public int? Shift { get; set; }
    public ModuleSimple Module { get; set; } = null!;
}