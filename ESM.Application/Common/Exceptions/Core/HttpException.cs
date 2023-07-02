using System.Net;
using System.Runtime.Serialization;
using ESM.Application.Common.Models;

namespace ESM.Application.Common.Exceptions.Core;

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