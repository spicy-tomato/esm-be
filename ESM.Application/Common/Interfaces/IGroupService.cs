using ESM.Domain.Entities;

namespace ESM.Application.Common.Interfaces;

public interface IGroupService
{
    public Guid CheckIfGroupExistAndReturnGuid(string groupId);
    
    public ShiftGroup CheckIfGroupExistAndReturnEntity(string groupId);
}