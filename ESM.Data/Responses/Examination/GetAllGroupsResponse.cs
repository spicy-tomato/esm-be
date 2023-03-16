using ESM.Data.Dtos.Examination;
using ESM.Data.Enums;
using ESM.Data.Interfaces;
using JetBrains.Annotations;

namespace ESM.Data.Responses.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class GetAllGroupsResponseResponseItem : IShiftGroup
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

    public class InternalModule
    {
        public string DisplayId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public InternalFaculty Faculty { get; set; } = null!;
    }

    public class InternalFaculty
    {
        public string Name { get; set; } = null!;
    }
}