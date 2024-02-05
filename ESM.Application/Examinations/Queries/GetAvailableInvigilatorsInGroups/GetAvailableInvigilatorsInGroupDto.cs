using System.Runtime.Serialization;
using AutoMapper;
using ESM.Domain.Entities;

namespace ESM.Application.Examinations.Queries.GetAvailableInvigilatorsInGroups;

[Serializable]
public class GetAvailableInvigilatorsInGroupsDto
    : Dictionary<string, List<GetAvailableInvigilatorsInGroupsItem.ResponseItem>>
{
    public GetAvailableInvigilatorsInGroupsDto() { }

    protected GetAvailableInvigilatorsInGroupsDto(SerializationInfo info, StreamingContext context) :
        base(info, context) { }
}

public record GetAvailableInvigilatorsInGroupsItem
{
    public Guid Id { get; init; }

    public ICollection<InternalFacultyShiftGroup> FacultyShiftGroups { get; init; } = null!;

    public record InternalFacultyShiftGroup
    {
        public Guid FacultyId { get; init; }

        public ICollection<InternalDepartmentShiftGroup> DepartmentShiftGroups { get; init; } = null!;
    }

    public record InternalDepartmentShiftGroup
    {
        public InternalUser? User { get; init; }
        public string? TemporaryInvigilatorName { get; init; }
        public Guid? DepartmentId { get; init; }
    }

    public record InternalUser
    {
        public Guid Id { get; init; }
        public string FullName { get; init; } = null!;
        public string? InvigilatorId { get; init; }
        public string? PhoneNumber { get; init; }
        public InternalDepartment? Department { get; init; }
    }

    public record InternalDepartment
    {
        public InternalFaculty? Faculty { get; init; }

        public string? Name { get; init; }
    }

    public record InternalFaculty
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
    }

    public abstract record ResponseItem
    {
        public bool IsPriority { get; init; }
    }

    public record VerifiedInvigilator : ResponseItem
    {
        public Guid Id { get; init; }
        public string FullName { get; init; } = null!;
        public string? InvigilatorId { get; init; }
        public string? PhoneNumber { get; init; }
        public string? FacultyName { get; init; }
        public string? DepartmentName { get; init; }
    }

    public record TemporaryInvigilator : ResponseItem
    {
        public string TemporaryName { get; init; } = null!;
        public Guid? DepartmentId { get; init; }
    }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<FacultyShiftGroup, InternalFacultyShiftGroup>();
            CreateMap<DepartmentShiftGroup, InternalDepartmentShiftGroup>();
            CreateMap<Teacher, InternalUser>();
            CreateMap<Department, InternalDepartment>();
            CreateMap<Faculty, InternalFaculty>();
        }
    }
}