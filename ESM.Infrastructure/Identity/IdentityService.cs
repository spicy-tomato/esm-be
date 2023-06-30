using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<IApplicationUser> _userManager;
    private readonly RoleManager<IApplicationRole> _roleManager;

    public IdentityService(UserManager<IApplicationUser> userManager, RoleManager<IApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id.ToString() == userId);

        return user.UserName;
    }

    public async Task<IApplicationUser?> FindUserByEmailAsync(string email) =>
        await _userManager.FindByEmailAsync(email);

    public async Task<IApplicationUser?> FindUserByIdAsync(string userId) =>
        await _userManager.FindByIdAsync(userId);

    public async Task<IApplicationUser?> FindUserByNameAsync(string name) =>
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
            throw new NotFoundException(nameof(ApplicationUser), userId);
        }

        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            throw new NotFoundException($"Cannot find role with name {roleName}");
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);

        return result.ToApplicationResult();
    }

    public async Task<Result<bool>> SetEmailAsync(IApplicationUser user, string email) =>
        (await _userManager.SetEmailAsync(user, email)).ToApplicationResult();

    public async Task<bool> CheckPasswordAsync(IApplicationUser user, string password) =>
        await _userManager.CheckPasswordAsync(user, password);


    public async Task<Result<bool>> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException(nameof(ApplicationUser), userId);
        }

        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

        return result.ToApplicationResult();
    }

    public async Task<string> GeneratePasswordResetTokenAsync(IApplicationUser user) =>
        await _userManager.GeneratePasswordResetTokenAsync(user);

    public async Task<Result<bool>> ResetPasswordAsync(IApplicationUser user, string token, string newPassword) =>
        (await _userManager.ResetPasswordAsync(user, token, newPassword)).ToApplicationResult();
}