using ESM.Application.Common.Models;
using ESM.Application.Groups.Commands.AssignInvigilatorsNumberToFaculty;
using ESM.Application.Groups.Commands.UpdateTemporaryNameToTeacher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESM.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class GroupController : ApiControllerBase
{
    #region Public Methods

    [HttpPatch("{groupId}", Name = nameof(UpdateTemporaryNameToTeacher))]
    public async Task<Result<bool>> UpdateTemporaryNameToTeacher(
        string groupId,
        [FromBody] UpdateTemporaryNameToTeacherRequest request)
    {
        var command = new UpdateTemporaryNameToTeacherCommand(groupId, request.UserId);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Assign invigilator number of a shift for a faculty
    /// </summary>
    /// <param name="numberOfInvigilator"></param>
    /// <param name="groupId"></param>
    /// <param name="facultyId"></param>
    /// <returns></returns>
    [HttpPatch("{groupId}/{facultyId}", Name = nameof(AssignInvigilatorsNumberToFaculty))]
    public async Task<Result<AssignInvigilatorsNumberToFacultyDto?>> AssignInvigilatorsNumberToFaculty(
        string groupId,
        string facultyId,
        [FromBody] int numberOfInvigilator)
    {
        var command = new AssignInvigilatorsNumberToFacultyCommand(groupId, facultyId, numberOfInvigilator);
        return await Mediator.Send(command);
    }

    #endregion
}