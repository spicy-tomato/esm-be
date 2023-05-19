using System.Net;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace ESM.Common.Core.Exceptions;

[Serializable]
[UsedImplicitly]
public class InternalServerErrorException : InnerException
{
    private const HttpStatusCode CODE = HttpStatusCode.InternalServerError;

    public InternalServerErrorException(string? message, Exception? innerException = null) : base(message,
        innerException) { }

    protected InternalServerErrorException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public override HttpException WrapException()
    {
        var errorResponse = new List<Error> { new(CODE.GetHashCode(), Message) };
        return new HttpException(CODE, errorResponse, Message, this);
    }
}