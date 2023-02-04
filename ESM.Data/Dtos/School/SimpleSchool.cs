using JetBrains.Annotations;

namespace ESM.Data.Dtos.School;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class SimpleSchool
{
    public string Id { get; set; } = null!;
    public string DisplayId { get; set; } = null!;
    public string Name { get; set; } = null!;
}