using AutoMapper;
using ESM.Domain.Entities;
using ESM.Domain.Identity;

namespace ESM.Application.Auth.Queries.MySummaryInfo;

public record InternalFaculty(Guid Id, string? DisplayId, string Name);

public record InternalDepartment(Guid Id, InternalFaculty? Faculty);

public record MySummaryInfoDto
{
    public Guid Id { get; init; }
    public string? FullName { get; init; }
    public bool? IsMale { get; init; }
    public InternalDepartment? Department { get; init; }
    public InternalFaculty? Faculty { get; init; }
    public IList<string> Roles { get; init; } = new List<string>();
    public string? PhoneNumber { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Faculty, InternalFaculty>();
            CreateMap<Department, InternalDepartment>();
            CreateMap<Teacher, MySummaryInfoDto>();
            CreateMap<ApplicationUser, MySummaryInfoDto>();
        }
    }
}