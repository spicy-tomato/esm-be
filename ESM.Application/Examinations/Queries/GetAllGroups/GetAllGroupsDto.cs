using ESM.Application.Common.Mappings;
using ESM.Domain.Dtos.Examination;
using ESM.Domain.Entities;
using ESM.Domain.Enums;
using ESM.Domain.Interfaces;
using JetBrains.Annotations;

namespace ESM.Application.Examinations.Queries.GetAllGroups;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class GetAllGroupsDto : IMapFrom<ShiftGroup>, IShiftGroup
{
    public Guid Id { get; set; }
    public ExamMethod Method { get; set; }
    public int InvigilatorsCount { get; set; }
    public int RoomsCount { get; set; }
    public DateTime StartAt { get; set; }
    public int? Shift { get; set; }
    public bool DepartmentAssign { get; set; }
    public InternalModule Module { get; set; } = null!;
    public Dictionary<string, ShiftGroupDataCell> AssignNumerate { get; set; } = new();

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
        public string Name { get; set; } = null!;
    }
}