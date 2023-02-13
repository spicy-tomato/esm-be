using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class InvigilatorShift
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int OrderIndex { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int ExaminationShiftId { get; set; }
    public ExaminationShift ExaminationShift { get; set; } = null!;

    [InverseProperty("Invigilator")]
    public Guid InvigilatorId { get; set; }

    public User Invigilator { get; set; } = null!;

    [InverseProperty("CreatedBy")]
    public Guid CreatedById { get; set; }

    public User CreatedBy { get; set; } = null!;
}