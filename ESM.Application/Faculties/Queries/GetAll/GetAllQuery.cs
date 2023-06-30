using AutoMapper;
using AutoMapper.QueryableExtensions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ESM.Application.Faculties.Queries.GetAll;

public record GetAllQuery : IRequest<Result<List<GetAllDto>>>;

public class GetAllQueryHandler : IRequestHandler<GetAllQuery, Result<List<GetAllDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Result<List<GetAllDto>>> Handle(GetAllQuery request,
        CancellationToken cancellationToken)
    {
        var result = _context.Faculties
            .Include(f => f.Departments.OrderBy(d => d.Name))
            .ProjectTo<GetAllDto>(_mapper.ConfigurationProvider)
            .ToList();

        return Task.FromResult(Result<List<GetAllDto>>.Get(result));
    }
}