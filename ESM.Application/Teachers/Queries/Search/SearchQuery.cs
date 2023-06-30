using AutoMapper;
using AutoMapper.QueryableExtensions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Data.Dtos.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ESM.Application.Teachers.Queries.Search;

public record SearchQuery(string FullName) : IRequest<Result<IEnumerable<UserSummary>>>;

public class SearchQueryHandler : IRequestHandler<SearchQuery, Result<IEnumerable<UserSummary>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Result<IEnumerable<UserSummary>>> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        var result = _context.Teachers
            .Where(u => string.IsNullOrWhiteSpace(request.FullName) ||
                        EF.Functions.Like(u.FullName, $"%{request.FullName}%"))
            .Take(20)
            .ProjectTo<UserSummary>(_mapper.ConfigurationProvider)
            .AsEnumerable();

        return Task.FromResult(Result<IEnumerable<UserSummary>>.Get(result));
    }
}