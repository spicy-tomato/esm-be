using System.ComponentModel.DataAnnotations.Schema;
using ESM.Data.Models;
using JetBrains.Annotations;

namespace ESM.Domain.Entities;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class InvigilatorShift
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public int OrderIndex { get; set; }

    public int Paid { get; set; }

    public User? Invigilator { get; set; }
    public Guid? InvigilatorId { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid ShiftId { get; set; }
    public Shift Shift { get; set; } = null!;
}