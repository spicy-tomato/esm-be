using JetBrains.Annotations;

namespace ESM.Application.Teachers.Commands.Update;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UpdateRequest
{
    public string Email { get; set; } = null!;
    public string? TeacherId { get; set; }
    public string FullName { get; set; } = null!;
    public bool IsMale { get; set; }
    public string? DepartmentId { get; set; }
}