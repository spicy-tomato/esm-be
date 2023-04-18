using JetBrains.Annotations;

namespace ESM.Data.Params.User;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class SearchParams
{
    public string? FullName { get; set; }
}