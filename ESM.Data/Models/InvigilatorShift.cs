using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class InvigilatorShift
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public int OrderIndex { get; set; }

    public int Paid { get; set; }

    public string? InvigilatorId { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid ShiftId { get; set; }
    public Shift Shift { get; set; } = null!;

    public Guid? CreatedById { get; set; }
    public User? CreatedBy { get; set; }
}