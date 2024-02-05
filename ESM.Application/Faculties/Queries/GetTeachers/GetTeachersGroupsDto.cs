using AutoMapper;
using ESM.Domain.Entities;

namespace ESM.Application.Faculties.Queries.GetTeachers;

public record GetTeachersDto
{
    public Guid Id { get; init; }
    public string? DisplayId { get; init; }
    public string Name { get; init; } = null!;
    public ICollection<InternalDepartment> Departments { get; init; } = null!;

    public record InternalDepartment
    {
        public Guid Id { get; init; }
        public string? DisplayId { get; init; }
        public string Name { get; init; } = null!;
    }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Teacher, GetTeachersDto>();
            CreateMap<Department, InternalDepartment>();
        }
    }
}