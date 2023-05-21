using ESM.Data.Enums;
using JetBrains.Annotations;

namespace ESM.Data.Responses.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class GetHandoverDataResponseItem
{
    public Guid Id { get; set; }
    public InternalShiftGroup ShiftGroup { get; set; } = null!;
    public InternalRoom Room { get; set; } = null!;
    public ICollection<InternalInvigilatorShift> InvigilatorShift { get; set; } = new List<InternalInvigilatorShift>();
    public Guid? HandedOverUserId { get; set; }
    public string? Report { get; set; }

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
        public InternalFaculty Faculty { get; set; } = null!;
    }

    public class InternalFaculty
    {
        public string? DisplayId { get; set; }
        public string Name { get; set; } = null!;
    }

    public class InternalRoom
    {
        public string DisplayId { get; set; } = null!;
    }

    public class InternalInvigilatorShift
    {
        public Guid Id { get; set; }
        public int OrderIndex { get; set; }
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
}