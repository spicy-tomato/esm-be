using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Implementations;
using ESM.API.Services;
using ESM.Common.Core.Exceptions;
using ESM.Core.API.Controllers;
using ESM.Data.Core.Response;
using ESM.Data.Dtos.Room;
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

    private readonly ApplicationContext _context;
    private readonly RoomRepository _roomRepository;

    #endregion

    #region Constructor

    public RoomController(IMapper mapper,
        ApplicationContext context,
        RoomRepository roomRepository) :
        base(mapper)
    {
        _context = context;
        _roomRepository = roomRepository;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Create a new room
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ConflictException"></exception>
    [HttpPost]
    public Result<RoomSummary?> Create(CreateRoomRequest request)
    {
        new CreateRoomRequestValidator().ValidateAndThrow(request);
        var room = Mapper.Map<Room>(request,
            opts => opts.AfterMap((_, des) =>
            {
                des.DisplayId = des.DisplayId.Trim();
            }));

        var existedRoom = _roomRepository.FindOne(r => r.DisplayId == room.DisplayId);
        if (existedRoom != null)
            throw new ConflictException("This room has been existed!");

        _roomRepository.Create(room);
        var response = Mapper.ProjectTo<RoomSummary>(_context.Rooms)
           .FirstOrDefault(f => f.Id == room.Id);

        return Result<RoomSummary?>.Get(response);
    }

    /// <summary>
    /// Import rooms
    /// </summary>
    /// <returns></returns>
    /// <exception cref="UnsupportedMediaTypeException"></exception>
    /// <exception cref="NotFoundException"></exception>
    [HttpPost("import")]
    public Result<bool> Import()
    {
        IFormFile file;
        try
        {
            file = Request.Form.Files[0];
        }
        catch (Exception)
        {
            throw new UnsupportedMediaTypeException();
        }

        var rooms = RoomService.Import(file);

        _roomRepository.CreateRange(
            rooms.Select(r => new Room
                {
                    DisplayId = r
                }
            ));

        _context.SaveChanges();

        return Result<RoomSummary?>.Get(true);
    }

    #endregion
}