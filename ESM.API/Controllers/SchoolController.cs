using AutoMapper;
using ESM.API.Repositories.Implementations;
using ESM.Core.API.Controllers;
using ESM.Data.Core.Response;
using ESM.Data.Models;
using ESM.Data.Request.Room;
using ESM.Data.Validations.Room;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESM.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RoomController : BaseController
{
    #region Properties

    private readonly RoomRepository _roomRepository;

    #endregion

    #region Constructor

    public RoomController(IMapper mapper, RoomRepository roomRepository) : base(mapper)
    {
        _roomRepository = roomRepository;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Create room
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Result<bool> Create([FromBody] CreateRoomRequest request)
    {
        new CreateRoomRequestValidator().ValidateAndThrow(request);
        var room = Mapper.Map<Room>(request);

        _roomRepository.Create(room);

        return Result<bool>.Get(true);
    }

    #endregion
}