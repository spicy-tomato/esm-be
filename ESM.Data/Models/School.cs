using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class School
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string? DisplayId { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();

    public ICollection<Department> Departments { get; set; } = new List<Department>();
}