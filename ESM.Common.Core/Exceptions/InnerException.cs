using System.Runtime.Serialization;

namespace ESM.Common.Core.Exceptions;

[Serializable]
public abstract class InnerException : Exception
{
    protected InnerException(string? message, Exception? innerException = null) : base(message, innerException) { }

    protected InnerException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public abstract HttpException WrapException();
}