using AutoMapper;
using AutoMapper.QueryableExtensions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Data.Dtos.User;
using MediatR;

namespace ESM.Application.Faculties.Queries.GetTeachers;

public record GetTeachersQuery(string Id) : IRequest<Result<List<UserSummary>>>;

public class GetTeachersQueryHandler : IRequestHandler<GetTeachersQuery, Result<List<UserSummary>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFacultyService _facultyService;

    public GetTeachersQueryHandler(IApplicationDbContext context, IMapper mapper, IFacultyService facultyService)
    {
        _context = context;
        _mapper = mapper;
        _facultyService = facultyService;
    }

    public Task<Result<List<UserSummary>>> Handle(GetTeachersQuery request, CancellationToken cancellationToken)
    {
        var facultyGuid = _facultyService.CheckIfExistAndReturnGuid(request.Id);

        var result = _context.Teachers
            .Where(t =>
                t.Department != null &&
                t.Department.FacultyId == facultyGuid)
            .ProjectTo<UserSummary>(_mapper.ConfigurationProvider)
            .ToList();

        return Task.FromResult(Result<List<UserSummary>>.Get(result));
    }
}