using ESM.Application.Common.Interfaces;
using ESM.Application.Shifts.Exceptions;
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
            throw new ShiftNotFoundException(groupId);
        }

        if (_context.ShiftGroups.FirstOrDefault(g => g.Id == groupGuid) == null)
        {
            throw new ShiftNotFoundException(groupId);
        }

        return groupGuid;
    }

    public ShiftGroup CheckIfGroupExistAndReturnEntity(string groupId)
    {
        if (!Guid.TryParse(groupId, out var groupGuid))
        {
            throw new ShiftNotFoundException(groupId);
        }

        var group = _context.ShiftGroups.FirstOrDefault(g => g.Id == groupGuid);
        if (group == null)
        {
            throw new ShiftNotFoundException(groupId);
        }

        return group;
    }
}