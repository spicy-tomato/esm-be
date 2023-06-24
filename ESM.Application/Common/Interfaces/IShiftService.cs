using ESM.Data.Models;

namespace ESM.Application.Common.Interfaces;

public interface IShiftService
{
    public Guid CheckIfShiftExistAndReturnGuid(string id);

    public Shift CheckIfShiftExistAndReturnEntity(string id);
}