using ESM.Application.Common.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IApplicationUser> _userManager;
    private readonly RoleManager<IApplicationRole> _roleManager;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger,
        ApplicationDbContext context,
        UserManager<IApplicationUser> userManager,
        RoleManager<IApplicationRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        var examinationDepartmentHeadRole = new ApplicationRole
        {
            Name = "ExaminationDepartmentHead"
        };
        var teacherRole = new ApplicationRole
        {
            Name = "Teacher"
        };

        if (await _roleManager.FindByNameAsync(examinationDepartmentHeadRole.Name) == null)
        {
            await _roleManager.CreateAsync(examinationDepartmentHeadRole);
        }

        if (await _roleManager.FindByNameAsync(teacherRole.Name) == null)
        {
            await _roleManager.CreateAsync(teacherRole);
        }

        var examinationDepartmentHead = new ApplicationUser { UserName = "admin" };

        if (_userManager.FindByNameAsync(examinationDepartmentHead.UserName) == null)
        {
            var result = await _userManager.CreateAsync(examinationDepartmentHead, "e10adc3949ba59abbe56e057f20f883e");
            if (result == IdentityResult.Success)
            {
                await _userManager.AddToRoleAsync(examinationDepartmentHead, examinationDepartmentHeadRole.Name);
            }
        }
    }
}