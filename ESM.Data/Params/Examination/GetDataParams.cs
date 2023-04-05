using JetBrains.Annotations;

namespace ESM.Data.Params.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class GetDataParams
{
    public bool? DepartmentAssign { get; set; }
    public int[]? Shift { get; set; }
}