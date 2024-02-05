using AutoMapper;
using ESM.Domain.Dtos.Module;
using ESM.Domain.Dtos.Room;
using ESM.Domain.Entities;
using ESM.Domain.Enums;
using JetBrains.Annotations;

namespace ESM.Application.Examinations.Queries.GetAllShiftsDetails;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record ShiftDetailsDto
{
    public InternalShiftGroup ShiftGroup { get; set; } = null!;
    public RoomSummary Room { get; set; } = null!;
    public int CandidatesCount { get; set; }
    public ICollection<InternalInvigilatorShift> InvigilatorShift { get; set; } = new List<InternalInvigilatorShift>();
    public bool IsDuplicated { get; set; }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public record InternalShiftGroup
    {
        public Guid Id { get; set; }
        public ExamMethod Method { get; set; }
        public DateTime StartAt { get; set; }
        public int? Shift { get; set; }
        public bool DepartmentAssign { get; set; }
        public ModuleSimple Module { get; set; } = null!;
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public record InternalInvigilatorShift
    {
        public Guid Id { get; set; }
        public int OrderIndex { get; set; }
        public InternalTeacher? Invigilator { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public record InternalTeacher
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? InvigilatorId { get; set; }
        public InternalDepartment? Department { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public record InternalDepartment
    {
        public string? DisplayId { get; set; }
        public string Name { get; set; } = null!;
        public InternalFaculty? Faculty { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public record InternalFaculty
    {
        public string? DisplayId { get; set; }
        public string Name { get; set; } = null!;
    }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Examination, ShiftDetailsDto>();
            CreateMap<ShiftGroup, InternalShiftGroup>();
            CreateMap<InvigilatorShift, InternalInvigilatorShift>();
            CreateMap<Teacher, InternalTeacher>();
            CreateMap<Department, InternalDepartment>();
            CreateMap<Faculty, InternalFaculty>();
        }
    }
}