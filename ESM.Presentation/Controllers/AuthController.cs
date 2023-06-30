using ESM.Application.Auth.Commands.ChangePassword;
using ESM.Application.Auth.Commands.Login;
using ESM.Application.Auth.Commands.ResetPassword;
using ESM.Application.Auth.Queries.MySummaryInfo;
using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Models;
using ESM.Data.Dtos;
using ESM.Data.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESM.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ApiControllerBase
{
    #region Public Methods

    /// <summary>
    /// Change password
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPatch("change-password")]
    [Authorize]
    public async Task<Result<bool>> ChangePassword(ChangePasswordCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedException"></exception>
    [HttpPost("login")]
    public async Task<Result<GeneratedToken>> Login(LoginCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Reset password
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPatch("reset-password")]
    [Authorize]
    public async Task<Result<bool>> ResetPassword([FromQuery] string userId)
    {
        var command = new ResetPasswordCommand(userId);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Get user summary
    /// </summary>
    /// <returns></returns>
    /// <exception cref="UnauthorizedException"></exception>
    [HttpGet("me")]
    [Authorize]
    public async Task<Result<UserSummary>> GetMySummaryInfo()
    {
        var command = new MySummaryInfoQuery();
        return await Mediator.Send(command);
    }

    #endregion
}