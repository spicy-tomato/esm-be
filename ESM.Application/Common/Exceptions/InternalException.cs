using System.Net;
using System.Runtime.Serialization;
using ESM.Application.Common.Models;
using JetBrains.Annotations;

namespace ESM.Application.Common.Exceptions;

[Serializable]
[UsedImplicitly]
public class InternalServerErrorException : InnerException
{
    private const HttpStatusCode Code = HttpStatusCode.InternalServerError;

    public InternalServerErrorException(string? message, Exception? innerException = null) : base(message,
        innerException) { }

    protected InternalServerErrorException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public override HttpException WrapException()
    {
        var errorResponse = new List<Error> { new(Code, Message) };
        return new HttpException(Code, errorResponse, Message, this);
    }
}