using JetBrains.Annotations;

namespace ESM.Data.Responses.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class GetAvailableInvigilatorsInShiftGroup
{
    public Guid Id { get; set; }

    public ICollection<InternalFacultyShiftGroup> FacultyShiftGroups { get; set; } =
        new List<InternalFacultyShiftGroup>();

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
        public string? PhoneNumber { get; set; }
        public InternalDepartment? Department { get; set; }
    }

    public class InternalDepartment
    {
        public InternalFaculty? Faculty { get; set; }
    }

    public class InternalFaculty
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class ResponseItem
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? InvigilatorId { get; set; }
        public bool IsPriority { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FacultyName { get; set; }
    }
}