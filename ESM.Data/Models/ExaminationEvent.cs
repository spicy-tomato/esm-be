using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESM.Data.Enums;
using JetBrains.Annotations;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ExaminationEvent
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public ExaminationStatus Status;

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreateAt;

    public Guid ExaminationId { get; set; }
    public Examination Examination { get; set; } = null!;
}