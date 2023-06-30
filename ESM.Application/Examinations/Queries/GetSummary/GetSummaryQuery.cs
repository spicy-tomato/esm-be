using AutoMapper;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Data.Dtos.Examination;
using ESM.Domain.Entities;
using MediatR;

namespace ESM.Application.Examinations.Queries.GetSummary;

public record GetSummaryQuery(string Id) : IRequest<Result<ExaminationSummary>>;

public class GetSummaryQueryHandler : IRequestHandler<GetSummaryQuery, Result<ExaminationSummary>>
{
    private readonly IMapper _mapper;
    private readonly IExaminationService _examinationService;

    public GetSummaryQueryHandler(IExaminationService examinationService, IMapper mapper)
    {
        _examinationService = examinationService;
        _mapper = mapper;
    }

    public Task<Result<ExaminationSummary>> Handle(GetSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var examination = _examinationService.CheckIfExaminationExistAndReturnEntity(request.Id);
        var summary = _mapper.Map<Examination, ExaminationSummary>(examination);

        return Task.FromResult(Result<ExaminationSummary>.Get(summary));
    }
}