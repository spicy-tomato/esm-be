using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id.ToString() == userId);

        return user.UserName;
    }

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

    public async Task<Result<bool>> AddUserToRole(Guid userId, string roleName)
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
}