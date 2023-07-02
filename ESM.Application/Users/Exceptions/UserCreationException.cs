using ESM.Application.Common.Exceptions.Core;

namespace ESM.Application.Users.Exceptions;

public class UserCreationException : InternalServerErrorException
{
    public UserCreationException() : base("Cannot create account") { }
}