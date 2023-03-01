using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ExaminationShift
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public int ExamsCount { get; set; }

    public int CandidatesCount { get; set; }

    public int InvigilatorsCount { get; set; }

    public DateTime StartAt { get; set; }


    public Guid ExaminationShiftGroupId { get; set; }
    public ExaminationShiftGroup ExaminationShiftGroup { get; set; } = null!;

    public Guid? RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public ICollection<InvigilatorShift> InvigilatorShift { get; set; } = new List<InvigilatorShift>();

    public ICollection<CandidateShift> CandidateShift { get; set; } = new List<CandidateShift>();
}