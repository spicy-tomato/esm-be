using System.Net;
using System.Runtime.Serialization;
using ESM.Application.Common.Models;
using JetBrains.Annotations;

namespace ESM.Application.Common.Exceptions.Core;

[Serializable]
[UsedImplicitly]
public abstract class NotFoundException : InnerException
{
    private const HttpStatusCode Code = HttpStatusCode.NotFound;

    protected NotFoundException(string? message, Exception? innerException = null) : base(message, innerException) { }

    protected NotFoundException(string? entityName, Guid entityId) : this(
        $"Cannot find {entityName} with ID {entityId}") { }

    protected NotFoundException(string? entityName, string fieldValue, string fieldName = "ID") : base(
        $"Cannot find {entityName} with {fieldName} {fieldValue}") { }

    protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public override HttpException WrapException()
    {
        var errorResponse = new List<Error> { new(Code, Message) };
        return new HttpException(Code, errorResponse, Message, this);
    }
}