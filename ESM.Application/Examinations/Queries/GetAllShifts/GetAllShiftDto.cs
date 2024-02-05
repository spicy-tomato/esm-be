using AutoMapper;
using ESM.Domain.Entities;
using ESM.Domain.Enums;
using JetBrains.Annotations;

namespace ESM.Application.Examinations.Queries.GetAllShifts;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record GetAllShiftDto
{
    public Guid Id { get; set; }
    public int ExamsCount { get; set; }
    public int CandidatesCount { get; set; }
    public int InvigilatorsCount { get; set; }
    public InternalRoom Room { get; set; } = null!;
    public InternalShiftGroup ShiftGroup { get; set; } = null!;

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public record InternalRoom
    {
        public string DisplayId { get; set; } = null!;
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public record InternalShiftGroup
    {
        public Guid Id { get; set; }
        public ExamMethod Method { get; set; }
        public DateTime StartAt { get; set; }
        public int? Shift { get; set; }
        public bool DepartmentAssign { get; set; }
        public InternalModule Module { get; set; } = null!;
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public record InternalModule
    {
        public string DisplayId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Credits { get; set; }
    }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Shift, GetAllShiftDto>();
            CreateMap<Room, InternalRoom>();
            CreateMap<ShiftGroup, InternalShiftGroup>();
            CreateMap<Module, InternalModule>();
        }
    }
}