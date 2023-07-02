using System.Net;
using System.Runtime.Serialization;
using ESM.Application.Common.Models;
using JetBrains.Annotations;

namespace ESM.Application.Common.Exceptions.Core;

[Serializable]
[UsedImplicitly]
public class UnauthorizedException : InnerException
{
    private const HttpStatusCode Code = HttpStatusCode.Unauthorized;

    public UnauthorizedException(string message = "Unauthorized", Exception? innerException = null) :
        base(message, innerException) { }

    protected UnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public override HttpException WrapException()
    {
        var errorResponse = new List<Error> { new(Code, Message) };
        return new HttpException(Code, errorResponse, Message, this);
    }
}