using ESM.Application.Common.Interfaces;
using ESM.Domain.Entities;

namespace Infrastructure.Identity;

public class ApplicationUser : IApplicationUser
{
    public Teacher? Teacher { get; set; }
    public Guid? TeacherId { get; set; }
}