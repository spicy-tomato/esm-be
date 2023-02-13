using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESM.Data.Enums;
using JetBrains.Annotations;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ExaminationShift
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public ExamMethod Method { get; set; }

    public int ExamsCount { get; set; }

    public int Credits { get; set; }

    public int DurationInMinutes { get; set; }
    public DateTime StartAt { get; set; }

    public DateTime EndAt => StartAt.AddMinutes(DurationInMinutes);

    public Guid ExaminationId { get; set; }
    public Examination Examination { get; set; } = null!;

    public Guid? ModuleId { get; set; }
    public Module Module { get; set; } = null!;

    public Guid? RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public ICollection<InvigilatorShift> InvigilatorShift { get; set; } = new List<InvigilatorShift>();

    public ICollection<CandidateShift> CandidateShift { get; set; } = new List<CandidateShift>();
}