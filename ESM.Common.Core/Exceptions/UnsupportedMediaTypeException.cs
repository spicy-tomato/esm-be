using System.Net;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace ESM.Common.Core.Exceptions;

[Serializable]
[UsedImplicitly]
public class UnsupportedMediaTypeException : InnerException
{
    private const HttpStatusCode CODE = HttpStatusCode.UnsupportedMediaType;

    public UnsupportedMediaTypeException(string message = "Unsupported media type", Exception? innerException = null) : base(message, innerException) { }

    public UnsupportedMediaTypeException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public override HttpException WrapException()
    {
        var errorResponse = new List<Error> { new(CODE.GetHashCode(), Message) };
        return new HttpException(CODE, errorResponse, Message, this);
    }
}