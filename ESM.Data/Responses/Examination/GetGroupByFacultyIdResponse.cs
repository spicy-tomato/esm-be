using ESM.Data.Dtos.User;
using JetBrains.Annotations;

namespace ESM.Data.Responses.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class GetGroupByFacultyIdResponseItem
{
    public Guid Id { get; set; }
    public Guid? DepartmentId { get; set; }
    public UserSimple? User { get; set; }
    public string? TemporaryInvigilatorName { get; set; }
    public InternalFacultyShiftGroup FacultyShiftGroup { get; set; } = null!;

    public class InternalFacultyShiftGroup
    {
        public Guid Id { get; set; }
        public InternalShiftGroup ShiftGroup { get; set; } = null!;
    }
    
    public class InternalShiftGroup
    {
        public DateTime StartAt { get; set; }
        public int? Shift { get; set; }
        public InternalModule Module { get; set; } = null!;
    }
    
    public class InternalModule
    {
        public string DisplayId { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}