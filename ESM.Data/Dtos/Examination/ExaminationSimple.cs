using JetBrains.Annotations;

namespace ESM.Data.Dtos.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ExaminationSimple
{
    public Guid Id { get; set; }
    public string DisplayId { get; set; } = null!;
    public string Name { get; set; } = null!;
}