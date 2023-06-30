using ESM.Application.Common.Mappings;
using ESM.Domain.Entities;
using JetBrains.Annotations;

namespace ESM.Application.Examinations.Queries.GetRelatedExaminations;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class RelatedExaminationDto : IMapFrom<Examination>
{
    public Guid Id { get; set; }
    public string DisplayId { get; set; } = null!;
    public string Name { get; set; } = null!;
}