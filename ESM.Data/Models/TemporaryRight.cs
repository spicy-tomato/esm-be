using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class TemporaryRight
{
    public Guid Id { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime? ExpiredAt { get; set; }

    [ForeignKey("GrantedBy")]
    public Guid GrantedById { get; set; }

    public User GrantedBy { get; set; } = null!;

    [ForeignKey("User")]
    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public int RightId { get; set; }
    public Right Right { get; set; } = null!;
}