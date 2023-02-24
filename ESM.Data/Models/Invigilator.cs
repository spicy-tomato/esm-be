using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Invigilator
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string? DisplayId { get; set; }

    public ICollection<InvigilatorExaminationModule> ExaminationModules { get; set; } =
        new List<InvigilatorExaminationModule>();

    public ICollection<InvigilatorShift> InvigilatorShift { get; set; } = new List<InvigilatorShift>();

    public User User { get; set; } = null!;
}