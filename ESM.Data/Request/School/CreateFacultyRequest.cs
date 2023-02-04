using JetBrains.Annotations;

namespace ESM.Data.Request.School;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class CreateSchoolRequest
{
    public string? DisplayId { get; set; }
    public string Name { get; set; } = null!;
}