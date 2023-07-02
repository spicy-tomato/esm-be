using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Exceptions.Core;
using ESM.Application.Common.Models;
using ESM.Application.Rooms.Commands.Create;
using ESM.Application.Rooms.Commands.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESM.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RoomController : ApiControllerBase
{
    #region Public Methods

    /// <summary>
    /// Create a new room
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="ConflictException"></exception>
    [HttpPost]
    public async Task<Result<Guid>> Create(CreateCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Import rooms
    /// </summary>
    /// <returns></returns>
    /// <exception cref="UnsupportedMediaTypeException"></exception>
    /// <exception cref="NotFoundException"></exception>
    [HttpPost("import")]
    public async Task<Result<bool>> Import([FromForm] ImportCommand command)
    {
        return await Mediator.Send(command);
    }

    #endregion
}