using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Exceptions.Core;
using ESM.Domain.Identity;

namespace ESM.Application.Roles.Exceptions;

public class RoleNotFoundException : NotFoundException
{
    public RoleNotFoundException(string roleName) : base(nameof(ApplicationRole), roleName) { }
}