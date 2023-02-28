using ESM.Data.Dtos.Module;
using ESM.Data.Dtos.Room;
using ESM.Data.Enums;
using JetBrains.Annotations;

namespace ESM.Data.Dtos.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ExaminationShiftSimple
{
    public int Id { get; set; }
    public ExamMethod Method { get; set; }
    public int ExamsCount { get; set; }
    public int CandidatesCount { get; set; }
    public int InvigilatorsCount { get; set; }
    public DateTime StartAt { get; set; }
    public int? Shift { get; set; }
    public ModuleSimple Module { get; set; } = null!;
    public RoomSummary Room { get; set; } = null!;
    public bool DepartmentAssign { get; set; }
}