using System.Net;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace ESM.Common.Core.Exceptions;

[Serializable]
[UsedImplicitly]
public class NotFoundException : InnerException
{
    private const HttpStatusCode CODE = HttpStatusCode.NotFound;

    public NotFoundException(string? message, Exception? innerException = null) : base(message, innerException) { }

    protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public override HttpException WrapException()
    {
        var errorResponse = new List<Error> { new(CODE, Message) };
        return new HttpException(CODE, errorResponse, Message, this);
    }
}