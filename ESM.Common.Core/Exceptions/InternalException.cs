using System.Net;
using JetBrains.Annotations;

namespace ESM.Common.Core.Exceptions;

[UsedImplicitly]
public class InternalServerErrorException : InnerException
{
    private const HttpStatusCode CODE = HttpStatusCode.InternalServerError;

    public InternalServerErrorException(string? message, Exception? innerException = null) : base(message,
        innerException) { }

    public override HttpException WrapException()
    {
        var errorResponse = new List<Error> { new(CODE.GetHashCode(), Message) };
        return new HttpException(CODE, errorResponse, Message, this);
    }
}