using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ESM.Domain.Entities;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Module
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(20)]
    public string DisplayId { get; set; } = null!;

    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public int Credits { get; set; }

    public int DurationInMinutes { get; set; }

    public Guid FacultyId { get; set; }
    public Faculty Faculty { get; set; } = null!;

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public ICollection<CandidateExaminationModule> CandidatesOfExamination { get; set; } =
        new List<CandidateExaminationModule>();
}