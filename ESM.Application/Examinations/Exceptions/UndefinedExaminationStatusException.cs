using ESM.Application.Common.Exceptions.Core;

namespace ESM.Application.Examinations.Exceptions;

public class UndefinedExaminationStatusException : BadRequestException
{
    public UndefinedExaminationStatusException() : base("Status is undefined") { }
}