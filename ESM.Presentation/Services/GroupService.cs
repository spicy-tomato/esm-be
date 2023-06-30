using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Interfaces;
using ESM.Domain.Entities;

namespace ESM.Presentation.Services;

public class GroupService : IGroupService
{
    #region Properties

    private readonly IApplicationDbContext _context;

    public GroupService(IApplicationDbContext context)
    {
        _context = context;
    }

    #endregion

    public Guid CheckIfGroupExistAndReturnGuid(string groupId)
    {
        if (!Guid.TryParse(groupId, out var groupGuid))
        {
            throw new NotFoundException(nameof(ShiftGroup), groupId);
        }

        if (_context.ShiftGroups.FirstOrDefault(g => g.Id == groupGuid) == null)
        {
            throw new NotFoundException(nameof(ShiftGroup), groupId);
        }

        return groupGuid;
    }

    public ShiftGroup CheckIfGroupExistAndReturnEntity(string groupId)
    {
        if (!Guid.TryParse(groupId, out var groupGuid))
        {
            throw new NotFoundException(nameof(ShiftGroup), groupId);
        }

        var group = _context.ShiftGroups.FirstOrDefault(g => g.Id == groupGuid);
        if (group == null)
        {
            throw new NotFoundException(nameof(ShiftGroup), groupId);
        }

        return group;
    }
}