using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESM.Data.Enums;
using JetBrains.Annotations;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Examination
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string DisplayId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? ExpectStartAt { get; set; }

    public DateTime? ExpectEndAt { get; set; }

    public ExaminationStatus Status { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; }

    public ICollection<ExaminationShiftGroup> ExaminationsShiftGroups { get; set; } = new List<ExaminationShiftGroup>();

    public ICollection<CandidateExaminationModule> CandidatesOfModule { get; set; } =
        new List<CandidateExaminationModule>();

    public ICollection<InvigilatorExaminationModule> InvigilatorsOfModule { get; set; } =
        new List<InvigilatorExaminationModule>();

    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;
}