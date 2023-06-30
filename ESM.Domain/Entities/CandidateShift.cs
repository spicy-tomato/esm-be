using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ESM.Domain.Entities;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class CandidateShift
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public int OrderIndex { get; set; }

    public Guid CandidateId { get; set; }
    public Candidate Candidate { get; set; } = null!;

    public Guid ShiftId { get; set; }
    public Shift Shift { get; set; } = null!;
}