namespace ESM.Application.Auth.Queries.MySummaryInfo;

public record InternalFaculty(Guid Id, string? DisplayId, string Name);

public record InternalDepartment(Guid Id, InternalFaculty? Faculty);

public class MySummaryInfoDto
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public bool? IsMale { get; set; }
    public InternalDepartment? Department { get; set; }
    public InternalFaculty? Faculty { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
    public string? PhoneNumber { get; set; }
}