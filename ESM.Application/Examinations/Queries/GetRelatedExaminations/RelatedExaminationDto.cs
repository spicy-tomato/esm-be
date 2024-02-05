using AutoMapper;
using ESM.Domain.Entities;

namespace ESM.Application.Examinations.Queries.GetRelatedExaminations;

public record RelatedExaminationDto
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