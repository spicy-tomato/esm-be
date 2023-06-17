using System.Net;
using System.Runtime.Serialization;
using ESM.Application.Common.Models;
using JetBrains.Annotations;

namespace ESM.Application.Common.Exceptions;

[Serializable]
[UsedImplicitly]
public class BadRequestException : InnerException
{
    private const HttpStatusCode CODE = HttpStatusCode.BadRequest;

    public BadRequestException(string? message, Exception? innerException = null) : base(message, innerException) { }

    protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public override HttpException WrapException()
    {
        var errorResponse = new List<Error> { new(CODE, Message) };
        return new HttpException(CODE, errorResponse, Message, this);
    }
}