using System.Net;
using System.Runtime.Serialization;
using ESM.Application.Common.Models;
using JetBrains.Annotations;

namespace ESM.Application.Common.Exceptions;

[Serializable]
[UsedImplicitly]
public class NotFoundException : InnerException
{
    private const HttpStatusCode CODE = HttpStatusCode.NotFound;

    public NotFoundException(string? message, Exception? innerException = null) : base(message, innerException) { }

    public NotFoundException(string? entityName, Guid entityId) :
        base($"Cannot find {entityName} with ID {entityId}") { }

    public NotFoundException(string? entityName, string id) : base($"Cannot find {entityName} with ID {id}") { }

    protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public override HttpException WrapException()
    {
        var errorResponse = new List<Error> { new(CODE, Message) };
        return new HttpException(CODE, errorResponse, Message, this);
    }
}