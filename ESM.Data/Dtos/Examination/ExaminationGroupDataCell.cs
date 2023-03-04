using JetBrains.Annotations;

namespace ESM.Data.Dtos.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ExaminationGroupDataCell
{
    public int Actual { get; set; }
    public int Calculated { get; set; }
    public int Maximum { get; set; }
}