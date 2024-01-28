using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESM.Domain.Identity;
using JetBrains.Annotations;

namespace ESM.Domain.Entities;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Shift
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public int ExamsCount { get; set; }

    public int CandidatesCount { get; set; }

    public int InvigilatorsCount { get; set; }

    [MaxLength(10000)]
    public string? Report { get; set; }

    public Guid? HandedOverUserId { get; set; }
    public ApplicationUser? HandedOverUser { get; set; }

    public Guid ShiftGroupId { get; set; }
    public ShiftGroup ShiftGroup { get; set; } = null!;

    public Guid? RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public ICollection<InvigilatorShift> InvigilatorShift { get; set; } = new List<InvigilatorShift>();

    public ICollection<CandidateShift> CandidateShift { get; set; } = new List<CandidateShift>();
}