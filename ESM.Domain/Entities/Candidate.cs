using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ESM.Domain.Entities;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Candidate
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(20)]
    public string DisplayId { get; set; } = null!;

    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public bool IsStudent { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }

    public ICollection<CandidateShift> CandidateShift { get; set; } = new List<CandidateShift>();

    public ICollection<CandidateExaminationModule> ExaminationModules { get; set; } =
        new List<CandidateExaminationModule>();
}