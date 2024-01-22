using ESM.Application.Common.Exceptions.Core;
using ESM.Domain.Identity;

namespace ESM.Application.Users.Exceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(string id) : base(nameof(ApplicationUser), id) { }

    public UserNotFoundException(Guid id) : base(nameof(ApplicationUser), id) { }
}