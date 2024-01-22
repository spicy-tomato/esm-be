using ESM.Domain.Dtos.Department;
using ESM.Domain.Dtos.Faculty;
using JetBrains.Annotations;

namespace ESM.Application.Teachers.Queries.Get;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record GetDto
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = null!;
    public string UserName { get; init; } = null!;
    public string? Email { get; init; }
    public bool IsMale { get; init; }
    public DateTime CreatedAt { get; init; }
    public string? InvigilatorId { get; init; }
    public DepartmentSummary? Department { get; init; }
    public FacultySummary? Faculty { get; init; }
    public string? PhoneNumber { get; init; }
}