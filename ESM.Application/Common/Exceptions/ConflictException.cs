using System.Net;
using System.Runtime.Serialization;
using ESM.Application.Common.Models;
using JetBrains.Annotations;

namespace ESM.Application.Common.Exceptions;

[Serializable]
[UsedImplicitly]
public class ConflictException : InnerException
{
    private const HttpStatusCode Code = HttpStatusCode.Conflict;

    public ConflictException(string? message, Exception? innerException = null) : base(message, innerException) { }

    protected ConflictException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public override HttpException WrapException()
    {
        var errorResponse = new List<Error> { new(Code, Message) };
        return new HttpException(Code, errorResponse, Message, this);
    }
}