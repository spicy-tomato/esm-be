using JetBrains.Annotations;

namespace ESM.Application.Examinations.Queries.GetStatistic;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record GetStatisticDto
{
    public Guid Id { get; init; }
    public string? DisplayId { get; init; }
    public string Name { get; init; } = null!;
    public DateTime? StartAt { get; init; }
    public DateTime? EndAt { get; init; }
    public double TimePercent { get; init; }
    public int NumberOfModules { get; init; }
    public int NumberOfModulesOver { get; init; }
    public int NumberOfShifts { get; init; }
    public int NumberOfShiftsOver { get; init; }
    public int NumberOfCandidates { get; init; }
    public int NumberOfInvigilators { get; init; }
}