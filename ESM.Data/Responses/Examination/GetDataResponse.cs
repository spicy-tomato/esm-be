using ESM.Data.Enums;
using JetBrains.Annotations;

namespace ESM.Data.Responses.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class GetDataResponseItem
{
    public Guid Id { get; set; }
    public int ExamsCount { get; set; }
    public int CandidatesCount { get; set; }
    public int InvigilatorsCount { get; set; }
    public InternalRoom Room { get; set; } = null!;
    public InternalShiftGroup ShiftGroup { get; set; } = null!;

    public class InternalRoom
    {
        public string DisplayId { get; set; } = null!;
    }
    
    public class InternalShiftGroup
    {
        public Guid Id { get; set; }
        public ExamMethod Method { get; set; }
        public DateTime StartAt { get; set; }
        public int? Shift { get; set; }
        public bool DepartmentAssign { get; set; }
        public InternalModule Module { get; set; } = null!;
    }
    
    public class InternalModule
    {
        public string DisplayId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Credits { get; set; }
    }
}