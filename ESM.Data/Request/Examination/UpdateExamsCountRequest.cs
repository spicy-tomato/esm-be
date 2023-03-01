using JetBrains.Annotations;

namespace ESM.Data.Request.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UpdateExamsCountRequest
{
    public Dictionary<string, int> Data = new();
}