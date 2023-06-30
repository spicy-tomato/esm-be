using ESM.Application.Common.Models;

namespace ESM.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<IApplicationUser?> FindUserByEmailAsync(string email);

    Task<IApplicationUser?> FindUserByIdAsync(string userId);

    Task<IApplicationUser?> FindUserByNameAsync(string name);

    Task<(Result<bool> Result, Guid UserId)> CreateUserAsync(string userName,
        string email,
        string password = "e10adc3949ba59abbe56e057f20f883e");

    Task<Result<bool>> AddUserToRoleAsync(Guid userId, string roleName);

    Task<Result<bool>> SetEmailAsync(IApplicationUser user, string email);

    Task<bool> CheckPasswordAsync(IApplicationUser user, string password);

    Task<Result<bool>> ChangePasswordAsync(string userId, string oldPassword, string newPassword);

    Task<string> GeneratePasswordResetTokenAsync(IApplicationUser user);

    Task<Result<bool>> ResetPasswordAsync(IApplicationUser user, string token, string newPassword);
}