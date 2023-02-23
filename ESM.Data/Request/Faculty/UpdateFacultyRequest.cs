using JetBrains.Annotations;

namespace ESM.Data.Request.Faculty;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UpdateFacultyRequest
{
    public string DisplayId { get; set; } = null!;
    public string Name { get; set; } = null!;
}