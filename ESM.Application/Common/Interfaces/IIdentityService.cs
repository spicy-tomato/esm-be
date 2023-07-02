using ESM.Application.Common.Models;
using ESM.Domain.Identity;

namespace ESM.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<ApplicationUser?> FindUserByEmailAsync(string email);

    Task<ApplicationUser?> FindUserByIdAsync(string userId);

    Task<ApplicationUser?> FindUserByNameAsync(string name);

    Task<(Result<bool> Result, Guid UserId)> CreateUserAsync(string userName,
        string email,
        string password = "e10adc3949ba59abbe56e057f20f883e");

    Task<Result<bool>> AddUserToRoleAsync(Guid userId, string roleName);
    
    Task<IList<string>> GetRolesAsync(ApplicationUser user);

    Task<Result<bool>> SetEmailAsync(ApplicationUser user, string email);

    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

    Task<Result<bool>> ChangePasswordAsync(string userId, string oldPassword, string newPassword);

    Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);

    Task<Result<bool>> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
}