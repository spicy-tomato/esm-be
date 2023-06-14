using AutoMapper;
using AutoMapper.QueryableExtensions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ESM.Application.Examinations.Query.GetRelatedExaminations;

public record GetRelatedExaminationsQuery : IRequest<Result<List<RelatedExaminationDto>>>
{
    public bool? IsActive { get; set; }
}

public class GetRelatedExaminationsQueryHandler
    : IRequestHandler<GetRelatedExaminationsQuery, Result<List<RelatedExaminationDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRelatedExaminationsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<List<RelatedExaminationDto>>> Handle(GetRelatedExaminationsQuery request,
        CancellationToken cancellationToken)
    {
        var query = request.IsActive switch
        {
            null => _context.Examinations,
            true => _context.Examinations.Where(e => 0 < e.Status && e.Status < ExaminationStatus.Closed),
            _ => _context.Examinations.Where(e => e.Status == ExaminationStatus.Closed)
        };

        var relatedExaminations = await query
           .ProjectTo<RelatedExaminationDto>(_mapper.ConfigurationProvider)
           .AsNoTracking()
           .ToListAsync(cancellationToken);

        return Result<List<RelatedExaminationDto>>.Get(relatedExaminations);
    }
}