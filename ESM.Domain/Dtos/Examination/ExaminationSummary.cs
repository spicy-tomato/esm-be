using ESM.Domain.Enums;

namespace ESM.Domain.Dtos.Examination;

public record ExaminationSummary
{
    public Guid Id { get; init; }
    public string DisplayId { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public DateTime? ExpectStartAt { get; init; }
    public DateTime? ExpectEndAt { get; init; }
    public ExaminationStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public Guid CreatedBy { get; init; }
}