using AutoMapper;
using ESM.Domain.Dtos.Examination;
using ESM.Domain.Entities;
using ESM.Domain.Enums;
using ESM.Domain.Interfaces;

namespace ESM.Application.Examinations.Queries.GetAllGroups;

public record GetAllGroupsDto : IShiftGroup
{
    public Guid Id { get; init; }
    public ExamMethod Method { get; init; }
    public int InvigilatorsCount { get; set; }
    public int RoomsCount { get; init; }
    public DateTime StartAt { get; init; }
    public int? Shift { get; init; }
    public bool DepartmentAssign { get; init; }
    public InternalModule Module { get; init; } = null!;
    public Dictionary<string, ShiftGroupDataCell> AssignNumerate { get; set; } = new();

    public record InternalModule
    {
        public string DisplayId { get; init; } = null!;
        public string Name { get; init; } = null!;
        public InternalFaculty Faculty { get; init; } = null!;
    }

    public record InternalFaculty
    {
        public string Name { get; init; } = null!;
    }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ShiftGroup, GetAllGroupsDto>();
            CreateMap<Module, InternalModule>();
            CreateMap<Faculty, InternalFaculty>();
        }
    }
}