using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Exceptions.Core;

namespace ESM.Application.Users.Exceptions;

public class UserHaveNoReferenceTeacherException : BadRequestException
{
    public UserHaveNoReferenceTeacherException() : base("This user has not reference teacher") { }
}