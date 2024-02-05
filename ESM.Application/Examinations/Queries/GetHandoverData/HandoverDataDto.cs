using AutoMapper;
using ESM.Domain.Entities;
using ESM.Domain.Enums;

namespace ESM.Application.Examinations.Queries.GetHandoverData;

public record HandoverDataDto
{
    public Guid Id { get; init; }
    public InternalShiftGroup ShiftGroup { get; init; } = null!;
    public InternalRoom Room { get; init; } = null!;
    public ICollection<InternalInvigilatorShift> InvigilatorShift { get; init; } = null!;
    public Guid? HandedOverUserId { get; init; }
    public string? Report { get; init; }

    public record InternalShiftGroup
    {
        public Guid Id { get; init; }
        public ExamMethod Method { get; init; }
        public DateTime StartAt { get; init; }
        public int? Shift { get; init; }
        public bool DepartmentAssign { get; init; }
        public InternalModule Module { get; init; } = null!;
    }

    public record InternalModule
    {
        public string DisplayId { get; init; } = null!;
        public string Name { get; init; } = null!;
        public InternalFaculty Faculty { get; init; } = null!;
    }

    public record InternalFaculty
    {
        public string? DisplayId { get; init; }
        public string Name { get; init; } = null!;
    }

    public record InternalRoom
    {
        public string DisplayId { get; init; } = null!;
    }

    public record InternalInvigilatorShift
    {
        public Guid Id { get; init; }
        public int OrderIndex { get; init; }
        public InternalUser? Invigilator { get; init; }
    }

    public record InternalUser
    {
        public Guid Id { get; init; }
        public string FullName { get; init; } = null!;
        public string? InvigilatorId { get; init; }
        public InternalDepartment? Department { get; init; }
    }

    public record InternalDepartment
    {
        public string? DisplayId { get; init; }
        public string Name { get; init; } = null!;
        public InternalFaculty? Faculty { get; init; }
    }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Shift, HandoverDataDto>();
            CreateMap<ShiftGroup, InternalShiftGroup>();
            CreateMap<Module, InternalModule>();
            CreateMap<Faculty, InternalFaculty>();
            CreateMap<Room, InternalRoom>();
            CreateMap<InvigilatorShift, InternalInvigilatorShift>();
            CreateMap<Teacher, InternalUser>();
            CreateMap<Department, InternalDepartment>();
        }
    }
}