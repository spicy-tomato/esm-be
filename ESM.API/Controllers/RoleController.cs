using AutoMapper;
using ESM.Common.Core.Exceptions;
using ESM.Core.API.Controllers;
using ESM.Data.Core.Response;
using ESM.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ESM.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RoleController : BaseController
{
    #region Properties

    private readonly RoleManager<Role> _roleManager;

    #endregion

    #region Constructor

    public RoleController(IMapper mapper, RoleManager<Role> roleManager) :
        base(mapper)
    {
        _roleManager = roleManager;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Create a new room
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    /// <exception cref="ConflictException"></exception>
    [HttpPost]
    public async Task<Result<bool>> Create([FromBody] string roleName)
    {
        // first we create Admin role
        var role = new Role
        {
            Name = roleName
        };
        await _roleManager.CreateAsync(role);

        return Result<bool>.Get(true);
    }

    #endregion
}