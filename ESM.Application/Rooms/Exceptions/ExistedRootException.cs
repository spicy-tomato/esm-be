using ESM.Application.Common.Exceptions.Core;

namespace ESM.Application.Rooms.Exceptions;

public class ExistedRootException : ConflictException
{
    public ExistedRootException() : base("This room has been existed!") { }
}