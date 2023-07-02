using System.Net;
using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Exceptions.Core;
using ESM.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ESM.Presentation.Filters;

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
                    { new(HttpStatusCode.InternalServerError, exception.Message) };
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