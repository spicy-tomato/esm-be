using System.Net;
using System.Runtime.Serialization;
using ESM.Application.Common.Models;

namespace ESM.Application.Common.Exceptions.Core;

[Serializable]
public abstract class BadRequestException : InnerException
{
    private const HttpStatusCode Code = HttpStatusCode.BadRequest;

    protected BadRequestException(string? message, Exception? innerException = null) : base(message, innerException) { }

    protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public override HttpException WrapException()
    {
        var errorResponse = new List<Error> { new(Code, Message) };
        return new HttpException(Code, errorResponse, Message, this);
    }
}