using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Department
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string? DisplayId { get; set; }

    public string Name { get; set; } = null!;

    public Guid? FacultyId { get; set; }
    public Faculty? Faculty { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
}