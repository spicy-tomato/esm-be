using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Application.Roles.Exceptions;
using ESM.Application.Users.Exceptions;
using ESM.Domain.Identity;
using ESM.Infrastructure.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ESM.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id.ToString() == userId);

        return user.UserName;
    }

    public async Task<ApplicationUser?> FindUserByEmailAsync(string email) =>
        await _userManager.FindByEmailAsync(email);

    public async Task<ApplicationUser?> FindUserByIdAsync(string userId) =>
        await _userManager.FindByIdAsync(userId);

    public async Task<ApplicationUser?> FindUserByNameAsync(string name) =>
        await _userManager.FindByNameAsync(name);

    public async Task<(Result<bool> Result, Guid UserId)> CreateUserAsync(string userName,
        string email,
        string password = "e10adc3949ba59abbe56e057f20f883e")
    {
        var user = new ApplicationUser
        {
            UserName = userName,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<Result<bool>> AddUserToRoleAsync(Guid userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            throw new RoleNotFoundException(roleName);
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);

        return result.ToApplicationResult();
    }

    public Task<IList<string>> GetRolesAsync(ApplicationUser user)
    {
        return _userManager.GetRolesAsync(user);
    }

    public async Task<Result<bool>> SetEmailAsync(ApplicationUser user, string email) =>
        (await _userManager.SetEmailAsync(user, email)).ToApplicationResult();

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password) =>
        await _userManager.CheckPasswordAsync(user, password);


    public async Task<Result<bool>> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

        return result.ToApplicationResult();
    }

    public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user) =>
        await _userManager.GeneratePasswordResetTokenAsync(user);

    public async Task<Result<bool>> ResetPasswordAsync(ApplicationUser user, string token, string newPassword) =>
        (await _userManager.ResetPasswordAsync(user, token, newPassword)).ToApplicationResult();
}