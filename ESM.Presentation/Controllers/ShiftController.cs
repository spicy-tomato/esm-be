using ESM.Application.Common.Models;
using ESM.Application.Shifts.Commands.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESM.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ShiftController : ApiControllerBase
{
    #region Public Methods

    /// <summary>
    /// Update shift data (handover report, handover person)
    /// </summary>
    /// <param name="shiftId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPatch("{shiftId}", Name = nameof(UpdateShift))]
    public async Task<Result<bool>> UpdateShift(string shiftId, [FromBody] UpdateRequest request)
    {
        var command = new UpdateCommand(shiftId, request);
        return await Mediator.Send(command);
    }

    #endregion
}