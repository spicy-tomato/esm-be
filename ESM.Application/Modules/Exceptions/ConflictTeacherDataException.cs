using ESM.Application.Common.Exceptions.Core;

namespace ESM.Application.Modules.Exceptions;

public class ConflictModuleDataException : ConflictException
{
    public ConflictModuleDataException(string conflictProperty) :
        base($"This module {conflictProperty} has been taken") { }
}