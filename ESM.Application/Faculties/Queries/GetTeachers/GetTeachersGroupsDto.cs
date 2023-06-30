using ESM.Application.Common.Mappings;
using ESM.Domain.Entities;
using JetBrains.Annotations;

namespace ESM.Application.Faculties.Queries.GetTeachers;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class GetTeachersDto : IMapFrom<Teacher>
{
    public Guid Id { get; set; }
    public string? DisplayId { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<InternalDepartment> Departments { get; set; } = null!;

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class InternalDepartment : IMapFrom<Department>
    {
        public Guid Id { get; set; }
        public string? DisplayId { get; set; }
        public string Name { get; set; } = null!;
    }
}