using ESM.Data.Dtos.Module;
using ESM.Data.Dtos.Room;
using ESM.Data.Enums;
using JetBrains.Annotations;

namespace ESM.Data.Responses.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class GetShiftResponseItem
{
    public DateTime StartAt { get; set; }
    public InternalShiftGroup ShiftGroup { get; set; } = null!;
    public RoomSummary Room { get; set; } = null!;
    public ICollection<InternalInvigilatorShift> InvigilatorShift { get; set; } = new List<InternalInvigilatorShift>();

    public class InternalShiftGroup
    {
        public Guid Id { get; set; }
        public ExamMethod Method { get; set; }
        public DateTime StartAt { get; set; }
        public int? Shift { get; set; }
        public bool DepartmentAssign { get; set; }
        public ModuleSimple Module { get; set; } = null!;
    }

    public class InternalInvigilatorShift
    {
        public Guid Id { get; set; }
        public int OrderIndex { get; set; }
        public string? InvigilatorId { get; set; }
        public InternalUser? Invigilator { get; set; }
    }

    public class InternalUser
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? InvigilatorId { get; set; }
        public InternalDepartment? Department { get; set; }
    }

    public class InternalDepartment
    {
        public string? DisplayId { get; set; }
        public string Name { get; set; } = null!;
        public InternalFaculty? Faculty { get; set; }
    }
    
    public class InternalFaculty
    {
        public string? DisplayId { get; set; }
        public string Name { get; set; } = null!;
    }
}