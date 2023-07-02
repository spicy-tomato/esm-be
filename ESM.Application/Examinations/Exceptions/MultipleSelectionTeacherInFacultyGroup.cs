using ESM.Application.Common.Exceptions.Core;

namespace ESM.Application.Examinations.Exceptions;

public class MultipleSelectionTeacherInFacultyGroup : BadRequestException
{
    public MultipleSelectionTeacherInFacultyGroup(Guid userId, Guid? facultyShiftGroupId) :
        base($"User ID {userId} is selected more than one time in faculty group ID {facultyShiftGroupId}") { }
}