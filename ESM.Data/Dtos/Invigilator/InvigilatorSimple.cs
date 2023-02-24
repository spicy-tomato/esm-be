using JetBrains.Annotations;

namespace ESM.Data.Dtos.Invigilator;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class InvigilatorSimple
{
    public Guid Id { get; set; }
    public string? DisplayId { get; set; }
}