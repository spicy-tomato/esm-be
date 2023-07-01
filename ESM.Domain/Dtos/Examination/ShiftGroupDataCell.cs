using JetBrains.Annotations;

namespace ESM.Domain.Dtos.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ShiftGroupDataCell
{
    public int Actual { get; set; }
    public int Calculated { get; set; }
    public int Maximum { get; set; }
}