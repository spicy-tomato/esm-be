using System.Runtime.Serialization;
using ESM.Application.Common.Mappings;
using ESM.Domain.Entities;
using JetBrains.Annotations;

namespace ESM.Application.Examinations.Queries.GetAvailableInvigilatorsInGroups;

[Serializable]
public class GetAvailableInvigilatorsInGroupsDto
    : Dictionary<string, List<GetAvailableInvigilatorsInGroupsItem.ResponseItem>>
{
    public GetAvailableInvigilatorsInGroupsDto() { }

    protected GetAvailableInvigilatorsInGroupsDto(SerializationInfo info, StreamingContext context) :
        base(info, context) { }
}

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class GetAvailableInvigilatorsInGroupsItem
{
    public Guid Id { get; set; }

    public ICollection<InternalFacultyShiftGroup> FacultyShiftGroups { get; set; } =
        new List<InternalFacultyShiftGroup>();

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalFacultyShiftGroup : IMapFrom<FacultyShiftGroup>
    {
        public Guid FacultyId { get; set; }

        public ICollection<InternalDepartmentShiftGroup> DepartmentShiftGroups { get; set; } =
            new List<InternalDepartmentShiftGroup>();
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalDepartmentShiftGroup : IMapFrom<DepartmentShiftGroup>
    {
        public InternalUser? User { get; set; }
        public string? TemporaryInvigilatorName { get; set; }
        public Guid? DepartmentId { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalUser : IMapFrom<Teacher>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? InvigilatorId { get; set; }
        public string? PhoneNumber { get; set; }
        public InternalDepartment? Department { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalDepartment : IMapFrom<Department>
    {
        public InternalFaculty? Faculty { get; set; }

        public string? Name { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalFaculty : IMapFrom<Faculty>
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