using ESM.Application.Common.Mappings;
using ESM.Data.Dtos.Module;
using ESM.Data.Dtos.Room;
using ESM.Data.Enums;
using ESM.Domain.Entities;
using JetBrains.Annotations;

namespace ESM.Application.Examinations.Queries.GetAllShiftsDetails;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ShiftDetailsDto : IMapFrom<Examination>
{
    public InternalShiftGroup ShiftGroup { get; set; } = null!;
    public RoomSummary Room { get; set; } = null!;
    public int CandidatesCount { get; set; }
    public ICollection<InternalInvigilatorShift> InvigilatorShift { get; set; } = new List<InternalInvigilatorShift>();
    public bool IsDuplicated { get; set; }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalShiftGroup : IMapFrom<ShiftGroup>
    {
        public Guid Id { get; set; }
        public ExamMethod Method { get; set; }
        public DateTime StartAt { get; set; }
        public int? Shift { get; set; }
        public bool DepartmentAssign { get; set; }
        public ModuleSimple Module { get; set; } = null!;
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalInvigilatorShift : IMapFrom<InvigilatorShift>
    {
        public Guid Id { get; set; }
        public int OrderIndex { get; set; }
        public InternalTeacher? Invigilator { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalTeacher : IMapFrom<Teacher>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? InvigilatorId { get; set; }
        public InternalDepartment? Department { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalDepartment : IMapFrom<Department>
    {
        public string? DisplayId { get; set; }
        public string Name { get; set; } = null!;
        public InternalFaculty? Faculty { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalFaculty : IMapFrom<Faculty>
    {
        public string? DisplayId { get; set; }
        public string Name { get; set; } = null!;
    }
}