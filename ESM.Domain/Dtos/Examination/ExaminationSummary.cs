using ESM.Domain.Dtos.User;
using ESM.Domain.Enums;
using JetBrains.Annotations;

namespace ESM.Domain.Dtos.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ExaminationSummary
{
    public Guid Id { get; set; }
    public string DisplayId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? ExpectStartAt { get; set; }
    public DateTime? ExpectEndAt { get; set; }
    public ExaminationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public UserSummary CreatedBy { get; set; } = null!;
}