using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Data.Dtos.Department;
using ESM.Data.Dtos.Faculty;
using MediatR;

namespace ESM.Application.Teachers.Queries.Get;

public record GetQuery(bool? IsInvigilator, bool? IsFaculty) : IRequest<Result<IEnumerable<GetDto>>>;

public class GetQueryHandler : IRequestHandler<GetQuery, Result<IEnumerable<GetDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Result<IEnumerable<GetDto>>> Handle(GetQuery request,
        CancellationToken cancellationToken)
    {
        // TODO: Refactor
        var result = _context.Users
            .Join(
                _context.Teachers,
                u => u.Id,
                t => t.UserId,
                (user, teacher) => new GetDto
                {
                    Id = user.Id,
                    FullName = teacher.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsMale = teacher.IsMale,
                    InvigilatorId = teacher.TeacherId,
                    Department = teacher.Department != null
                        ? new DepartmentSummary
                        {
                            Id = teacher.Department.Id,
                            DisplayId = teacher.Department.DisplayId,
                            Name = teacher.Department.Name
                        }
                        : null,
                    Faculty = teacher.Department != null
                        ? new FacultySummary
                        {
                            Id = teacher.Department.Id,
                            DisplayId = teacher.Department.DisplayId,
                            Name = teacher.Department.Name
                        }
                        : null,
                    PhoneNumber = user.PhoneNumber
                })
            .Where(u =>
                (request.IsInvigilator == true && (request.IsFaculty == null || request.IsFaculty == false)) ||
                (request.IsFaculty == true && (request.IsInvigilator == null || request.IsInvigilator == false) &&
                 u.Department == null && u.Faculty != null)
            )
            .AsEnumerable();

        return Task.FromResult(Result<IEnumerable<GetDto>>.Get(result));
    }
}