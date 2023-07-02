namespace ESM.Application.Auth.Queries.MySummaryInfo;

public record InternalFaculty(Guid Id, string? DisplayId, string Name);

public record InternalDepartment(Guid Id, string? DisplayId, string Name, InternalFaculty? Faculty);

public record MySummaryInfoDto(Guid Id,
    string FullName,
    string UserName,
    string? Email,
    bool IsMale,
    DateTime CreatedAt,
    string? TeacherId,
    InternalDepartment? Department,
    InternalFaculty? Faculty,
    IList<string> Roles,
    string? PhoneNumber);