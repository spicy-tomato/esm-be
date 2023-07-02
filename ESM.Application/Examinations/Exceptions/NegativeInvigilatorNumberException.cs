using ESM.Application.Common.Exceptions.Numeric;

namespace ESM.Application.Examinations.Exceptions;

public class NegativeInvigilatorNumberException : NegativeNumberException
{
    public NegativeInvigilatorNumberException() : base("Invigilator") { }
}