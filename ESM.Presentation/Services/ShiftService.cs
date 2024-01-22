using ESM.Application.Common.Exceptions.Core;
using ESM.Application.Common.Interfaces;
using ESM.Application.Shifts.Exceptions;
using ESM.Domain.Entities;

namespace ESM.Presentation.Services;

public class ShiftService : IShiftService
{
    private readonly IApplicationDbContext _context;

    public ShiftService(IApplicationDbContext context)
    {
        _context = context;
    }

    public Shift CheckIfShiftExistAndReturnEntity(string id)
    {
        TryParseGuid(id, out var guid);
        var shift = Find(guid);

        return shift;
    }

    private static void TryParseGuid(string id, out Guid guid)
    {
        if (!Guid.TryParse(id, out guid))
        {
            throw CreateNotFoundException(id);
        }
    }

    private Shift Find(Guid guid)
    {
        var shift = _context.Shifts.FirstOrDefault(s => s.Id == guid);
        if (shift == null)
        {
            throw CreateNotFoundException(guid.ToString());
        }

        return shift;
    }

    private static NotFoundException CreateNotFoundException(string id)
    {
        return new ShiftNotFoundException(id);
    }
}