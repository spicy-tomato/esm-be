using ESM.Data.Dtos.School;
using JetBrains.Annotations;

namespace ESM.Data.Dtos.Faculty;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class SimpleFaculty
{
    public Guid Id { get; set; }
    public string DisplayId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public SimpleSchool School { get; set; } = null!;
}