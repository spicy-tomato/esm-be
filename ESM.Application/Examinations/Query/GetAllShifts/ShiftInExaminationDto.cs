using ESM.Application.Common.Mappings;
using ESM.Data.Enums;
using ESM.Domain.Entities;
using JetBrains.Annotations;

namespace ESM.Application.Examinations.Query.GetAllShifts;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ShiftInExaminationDto : IMapFrom<Examination>
{
    public Guid Id { get; set; }
    public int ExamsCount { get; set; }
    public int CandidatesCount { get; set; }
    public int InvigilatorsCount { get; set; }
    public InternalRoom Room { get; set; } = null!;
    public InternalShiftGroup ShiftGroup { get; set; } = null!;

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalRoom : IMapFrom<Room>
    {
        public string DisplayId { get; set; } = null!;
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalShiftGroup : IMapFrom<ShiftGroup>
    {
        public Guid Id { get; set; }
        public ExamMethod Method { get; set; }
        public DateTime StartAt { get; set; }
        public int? Shift { get; set; }
        public bool DepartmentAssign { get; set; }
        public InternalModule Module { get; set; } = null!;
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalModule : IMapFrom<Module>
    {
        public string DisplayId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Credits { get; set; }
    }
}