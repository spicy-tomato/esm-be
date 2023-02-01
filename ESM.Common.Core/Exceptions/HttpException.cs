using System.Net;

namespace ESM.Common.Core.Exceptions;

public class HttpException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public IEnumerable<Error> Errors { get; }

    public HttpException(HttpStatusCode statusCode,
        IEnumerable<Error> errors,
        string message = "",
        Exception? innerException = null) : base(message, innerException)
    {
        StatusCode = statusCode;
        Errors = errors;
    }
}