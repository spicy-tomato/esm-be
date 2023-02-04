using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Module
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string DisplayId { get; set; } = null!;

    public Guid? FacultyId { get; set; }
    public Faculty Faculty { get; set; } = null!;

    public ICollection<CandidateExaminationModule> CandidatesOfExamination { get; set; } =
        new List<CandidateExaminationModule>();

    public ICollection<InvigilatorExaminationModule> InvigilatorsOfExamination { get; set; } =
        new List<InvigilatorExaminationModule>();
}