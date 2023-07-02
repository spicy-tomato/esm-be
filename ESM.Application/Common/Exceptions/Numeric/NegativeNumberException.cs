using ESM.Application.Common.Exceptions.Core;

namespace ESM.Application.Common.Exceptions.Numeric;

public class NegativeNumberException : BadRequestException
{
    public NegativeNumberException(string property) : base($"{property} number cannot be negative!") { }
}