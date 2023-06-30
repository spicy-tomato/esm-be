namespace ESM.Application.Common.Interfaces;

public interface IUserService
{
    public bool UserExist(Guid id);
    
    public bool UserExist(string id);
    
    public Task<IApplicationUser> CheckIfExistAndReturnEntity(string id);
}