using JetBrains.Annotations;

namespace ESM.Data.Responses.Faculty;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class GetAllResponseItem
{
    public Guid Id { get; set; }
    public string? DisplayId { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<InternalDepartment> Departments { get; set; } = null!;

    public class InternalDepartment
    {
        public Guid Id { get; set; }
        public string? DisplayId { get; set; }
        public string Name { get; set; } = null!;
    }
}