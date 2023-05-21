using System.Net;
using System.Runtime.Serialization;

namespace ESM.Common.Core.Exceptions;

[Serializable]
public class HttpException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public IEnumerable<Error> Errors { get; } = new List<Error>();

    public HttpException(HttpStatusCode statusCode,
        IEnumerable<Error> errors,
        string message = "",
        Exception? innerException = null) : base(message, innerException)
    {
        StatusCode = statusCode;
        Errors = errors;
    }

    protected HttpException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        StatusCode = HttpStatusCode.Accepted;
    }
}