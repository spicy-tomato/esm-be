using JetBrains.Annotations;

namespace ESM.Data.Request.Module;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class CreateModuleRequest
{
    public string DisplayId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Guid? DepartmentId { get; set; }
}