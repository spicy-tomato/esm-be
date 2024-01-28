using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ESM.Domain.Entities;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Room
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(20)]
    public string DisplayId { get; set; } = null!;

    public int? Capacity { get; set; }

    public ICollection<Shift> Shift { get; set; } = new List<Shift>();
}