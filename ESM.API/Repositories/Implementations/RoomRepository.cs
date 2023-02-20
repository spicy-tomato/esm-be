using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ESM.API.Repositories.Implementations;

public class RoomRepository : RepositoryBase<Room>, IRoomRepository
{
    #region Constructor

    public RoomRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion

    public Task<List<string>> GetIdsAsync() => Context.Rooms.Select(r => r.DisplayId).ToListAsync();
}