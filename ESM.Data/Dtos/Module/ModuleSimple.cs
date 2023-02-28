using JetBrains.Annotations;

namespace ESM.Data.Dtos.Module;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ModuleSimple
{
    public Guid Id { get; set; }
    public string DisplayId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Credits { get; set; }
}