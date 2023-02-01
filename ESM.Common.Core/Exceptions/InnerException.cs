namespace ESM.Common.Core.Exceptions;

public abstract class InnerException : Exception
{
    protected InnerException(string? message, Exception? innerException = null) : base(message, innerException) { }

    public abstract HttpException WrapException();
}