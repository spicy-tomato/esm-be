using ESM.Application.Common.Exceptions.Core;

namespace ESM.Application.Examinations.Exceptions;

public class DepartmentGroupNotFoundInFacultyGroupException : BadRequestException
{
    public DepartmentGroupNotFoundInFacultyGroupException(string departmentShiftGroupId, string facultyShiftGroupId) :
        base($"Department group ID {departmentShiftGroupId} is not exist in faculty group ID {facultyShiftGroupId}") { }
}