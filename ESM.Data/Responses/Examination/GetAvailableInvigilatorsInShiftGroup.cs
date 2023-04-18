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
        public Guid? DepartmentId { get; set; }
    }

    public class InternalUser
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? InvigilatorId { get; set; }
        public string? PhoneNumber { get; set; }
        public InternalDepartment? Department { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalDepartment
    {
        public InternalFaculty? Faculty { get; set; }

        public string? Name { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalFaculty
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class ResponseItem
    {
        public bool IsPriority { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class VerifiedInvigilator : ResponseItem
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? InvigilatorId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FacultyName { get; set; }
        public string? DepartmentName { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class TemporaryInvigilator : ResponseItem
    {
        public string TemporaryName { get; set; } = null!;
        public Guid? DepartmentId { get; set; }
    }
}