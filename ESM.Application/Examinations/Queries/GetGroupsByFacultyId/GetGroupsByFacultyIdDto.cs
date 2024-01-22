using AutoMapper;
using ESM.Application.Common.Mappings;
using ESM.Domain.Dtos.User;
using ESM.Domain.Entities;
using JetBrains.Annotations;

namespace ESM.Application.Examinations.Queries.GetGroupsByFacultyId;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record GetGroupsByFacultyIdDto : IMapFrom<Examination>
{
    public Guid Id { get; set; }
    public Guid? DepartmentId { get; set; }
    public UserSimple? User { get; set; }
    public string? TemporaryInvigilatorName { get; set; }
    public InternalFacultyShiftGroup FacultyShiftGroup { get; set; } = null!;

    public record InternalFacultyShiftGroup : IMapFrom<FacultyShiftGroup>
    {
        public Guid Id { get; init; }
        public InternalShiftGroup ShiftGroup { get; init; } = null!;
    }

    public record InternalShiftGroup : IMapFrom<ShiftGroup>
    {
        public DateTime StartAt { get; init; }
        public int? Shift { get; init; }
        public InternalModule Module { get; init; } = null!;
    }

    public record InternalModule : IMapFrom<Module>
    {
        public string DisplayId { get; init; } = null!;
        public string Name { get; init; } = null!;
    }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Examination, GetGroupsByFacultyIdDto>();
            CreateMap<FacultyShiftGroup, InternalFacultyShiftGroup>();
            CreateMap<ShiftGroup, InternalShiftGroup>();
            CreateMap<Module, InternalModule>();
        }
    }
}