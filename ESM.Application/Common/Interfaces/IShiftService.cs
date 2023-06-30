using ESM.Domain.Entities;

namespace ESM.Application.Common.Interfaces;

public interface IShiftService
{
    public Shift CheckIfShiftExistAndReturnEntity(string id);
}