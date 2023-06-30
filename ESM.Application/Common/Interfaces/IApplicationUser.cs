using Microsoft.AspNetCore.Identity;

namespace ESM.Application.Common.Interfaces;

public abstract class IApplicationUser : IdentityUser<Guid> { }