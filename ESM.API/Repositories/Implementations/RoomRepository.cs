using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Interface;
using ESM.Data.Models;

namespace ESM.API.Repositories.Implementations;

public class RoomRepository : RepositoryBase<Room>, IRoomRepository
{
    #region Constructor

    public RoomRepository(ApplicationContext context, IMapper mapper) : base(context, mapper) { }

    #endregion

    public IEnumerable<string> GetIds() => Context.Rooms.Select(r => r.DisplayId);
}