using ESM.Application.Common.Mappings;
using ESM.Domain.Entities;
using JetBrains.Annotations;

namespace ESM.Application.Faculties.Queries.GetAll;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class GetAllDto : IMapFrom<Faculty>
{
    public Guid Id { get; set; }
    public string? DisplayId { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<InternalDepartment> Departments { get; set; } = null!;

    public class InternalDepartment : IMapFrom<Department>
    {
        public Guid Id { get; set; }
        public string? DisplayId { get; set; }
        public string Name { get; set; } = null!;
    }
}