using JetBrains.Annotations;

namespace ESM.Data.Request.Faculty;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class CreateFacultyRequest
{
    public string? DisplayId { get; set; }
    public string Name { get; set; } = null!;
}