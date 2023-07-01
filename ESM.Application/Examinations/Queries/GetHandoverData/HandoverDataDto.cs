using ESM.Application.Common.Mappings;
using ESM.Domain.Entities;
using ESM.Domain.Enums;
using JetBrains.Annotations;

namespace ESM.Application.Examinations.Queries.GetHandoverData;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class HandoverDataDto : IMapFrom<Shift>
{
    public Guid Id { get; set; }
    public InternalShiftGroup ShiftGroup { get; set; } = null!;
    public InternalRoom Room { get; set; } = null!;
    public ICollection<InternalInvigilatorShift> InvigilatorShift { get; set; } = new List<InternalInvigilatorShift>();
    public Guid? HandedOverUserId { get; set; }
    public string? Report { get; set; }

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
        public InternalFaculty Faculty { get; set; } = null!;
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalFaculty : IMapFrom<Faculty>
    {
        public string? DisplayId { get; set; }
        public string Name { get; set; } = null!;
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalRoom : IMapFrom<Room>
    {
        public string DisplayId { get; set; } = null!;
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalInvigilatorShift : IMapFrom<InvigilatorShift>
    {
        public Guid Id { get; set; }
        public int OrderIndex { get; set; }
        public InternalUser? Invigilator { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalUser : IMapFrom<Teacher>
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
}