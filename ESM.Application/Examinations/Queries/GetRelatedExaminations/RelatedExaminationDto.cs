using AutoMapper;
using ESM.Application.Common.Mappings;
using ESM.Domain.Entities;

namespace ESM.Application.Examinations.Queries.GetRelatedExaminations;

public record RelatedExaminationDto : IMapFrom<Examination>
{
    public Guid Id { get; init; }
    public string DisplayId { get; init; } = null!;
    public string Name { get; init; } = null!;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Examination, RelatedExaminationDto>();
        }
    }
}