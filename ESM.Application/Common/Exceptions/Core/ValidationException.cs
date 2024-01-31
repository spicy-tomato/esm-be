using ESM.Application.Common.Models;
using FluentValidation.Results;

namespace ESM.Application.Common.Exceptions.Core;

[Serializable]
public class ValidationException : BadRequestException
{
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures have occurred.")
    {
        Errors = failures
            .Select(f => new Error(f.PropertyName, f.ErrorMessage));
    }

    private IEnumerable<Error> Errors { get; }

    public override HttpException WrapException()
    {
        return new HttpException(Code, Errors, Message, this);
    }
}