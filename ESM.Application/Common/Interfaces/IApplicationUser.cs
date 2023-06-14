using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public abstract class IApplicationUser : IdentityUser<Guid> { }