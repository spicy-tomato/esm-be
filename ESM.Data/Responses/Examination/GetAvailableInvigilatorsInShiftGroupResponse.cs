using JetBrains.Annotations;

namespace ESM.Data.Responses.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class GetAvailableInvigilatorsInShiftGroupResponse
{
    public ICollection<InternalShiftGroup> ShiftGroups { get; set; } = new List<InternalShiftGroup>();

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalShiftGroup
    {
        public Guid Id { get; set; }
        public ICollection<InternalFacultyShiftGroup> FacultyShiftGroups { get; set; } =
            new List<InternalFacultyShiftGroup>();
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalFacultyShiftGroup
    {
        public Guid FacultyId { get; set; }
        public ICollection<InternalDepartmentShiftGroup> DepartmentShiftGroups { get; set; } =
            new List<InternalDepartmentShiftGroup>();
    }

    public class InternalDepartmentShiftGroup
    {
        public InternalUser? User { get; set; }
        public string? TemporaryInvigilatorName { get; set; }
    }

    public class InternalUser
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? InvigilatorId { get; set; }
    }
}