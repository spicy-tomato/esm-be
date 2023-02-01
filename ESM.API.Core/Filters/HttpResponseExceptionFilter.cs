using System.Net;
using ESM.Common.Core;
using ESM.Common.Core.Exceptions;
using ESM.Data.Core.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ESM.Core.API.Filters;

public class HttpResponseExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        HttpException exceptionToHandle;

        switch (exception)
        {
            case HttpException httpException:
                exceptionToHandle = httpException;
                break;
            case InnerException innerException:
                exceptionToHandle = innerException.WrapException();
                break;
            case FluentValidation.ValidationException validationException:
            {
                var errorResponse = validationException.Errors
                   .Select(e => new Error(e.PropertyName, e.ErrorMessage));
                exceptionToHandle = new HttpException(HttpStatusCode.BadRequest, errorResponse);
                break;
            }
            default:
            {
                var errorResponse = new List<Error>
                    { new(HttpStatusCode.InternalServerError.GetHashCode(), exception.Message) };
                exceptionToHandle = new HttpException(HttpStatusCode.InternalServerError,
                    errorResponse,
                    exception.Message,
                    exception);
                break;
            }
        }

        var response = new Result<bool>
        {
            Success = false,
            Errors = exceptionToHandle.Errors
        };

        context.Result = new JsonResult(response)
        {
            StatusCode = (int?)exceptionToHandle.StatusCode
        };
        context.ExceptionHandled = true;
    }
}