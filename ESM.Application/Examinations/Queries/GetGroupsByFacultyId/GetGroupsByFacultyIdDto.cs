using ESM.Application.Common.Mappings;
using ESM.Data.Dtos.User;
using ESM.Domain.Entities;
using JetBrains.Annotations;

namespace ESM.Application.Examinations.Queries.GetGroupsByFacultyId;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class GetGroupsByFacultyIdDto : IMapFrom<Examination>
{
    public Guid Id { get; set; }
    public Guid? DepartmentId { get; set; }
    public UserSimple? User { get; set; }
    public string? TemporaryInvigilatorName { get; set; }
    public InternalFacultyShiftGroup FacultyShiftGroup { get; set; } = null!;

    public class InternalFacultyShiftGroup : IMapFrom<FacultyShiftGroup>
    {
        public Guid Id { get; set; }
        public InternalShiftGroup ShiftGroup { get; set; } = null!;
    }

    public class InternalShiftGroup : IMapFrom<ShiftGroup>
    {
        public DateTime StartAt { get; set; }
        public int? Shift { get; set; }
        public InternalModule Module { get; set; } = null!;
    }

    public class InternalModule : IMapFrom<Module>
    {
        public string DisplayId { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}