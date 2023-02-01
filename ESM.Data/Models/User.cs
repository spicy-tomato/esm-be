using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class User : IdentityUser
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }
}