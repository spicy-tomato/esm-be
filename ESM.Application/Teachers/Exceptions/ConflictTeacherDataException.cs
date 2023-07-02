using System.Net;
using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Exceptions.Core;
using ESM.Application.Common.Models;

namespace ESM.Application.Teachers.Exceptions;

public class ConflictTeacherDataException : HttpException
{
    public ConflictTeacherDataException(IEnumerable<Error> errorList) : base(HttpStatusCode.Conflict, errorList) { }
}