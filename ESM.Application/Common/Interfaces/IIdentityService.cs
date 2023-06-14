using ESM.Application.Common.Models;

namespace ESM.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<(Result<bool> Result, Guid UserId)> CreateUserAsync(string userName,
        string email,
        string password = "e10adc3949ba59abbe56e057f20f883e");

    Task<Result<bool>> AddUserToRole(Guid userId, string roleName);
}