using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESM.Domain.Common;
using ESM.Domain.Enums;
using JetBrains.Annotations;

namespace ESM.Domain.Entities;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Examination : BaseAuditableEntity
{
    [MaxLength(20)]
    public string? DisplayId { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(200)]
    public string? Description { get; set; }

    public DateTime? ExpectStartAt { get; set; }

    public DateTime? ExpectEndAt { get; set; }

    public ExaminationStatus Status { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; }

    public ICollection<ShiftGroup> ShiftGroups { get; set; } = new List<ShiftGroup>();

    public ICollection<CandidateExaminationModule> CandidatesOfModule { get; set; } =
        new List<CandidateExaminationModule>();

    public ICollection<ExaminationEvent> Events { get; set; } = new List<ExaminationEvent>();
}