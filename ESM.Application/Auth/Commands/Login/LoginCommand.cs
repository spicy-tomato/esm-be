using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Exceptions.Core;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Domain.Dtos;
using ESM.Domain.Identity;
using MediatR;

namespace ESM.Application.Auth.Commands.Login;

public record LoginCommand(string UserName, string Password) : IRequest<Result<GeneratedToken>>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<GeneratedToken>>
{
    private readonly IIdentityService _identityService;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(IIdentityService identityService, IJwtService jwtService)
    {
        _identityService = identityService;
        _jwtService = jwtService;
    }

    public async Task<Result<GeneratedToken>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await GetUser(request.UserName);

        if (user is null || await WrongPassword(user, request.Password))
        {
            throw new UnauthorizedException();
        }

        var token = _jwtService.CreateToken(user);
        return Result<GeneratedToken>.Get(token);
    }

    private async Task<ApplicationUser?> GetUser(string userName)
    {
        var isEmail = userName.Contains('@');

        var user = isEmail
            ? await _identityService.FindUserByEmailAsync(userName)
            : await _identityService.FindUserByNameAsync(userName);

        return user;
    }

    private async Task<bool> WrongPassword(ApplicationUser user, string password) =>
        !await _identityService.CheckPasswordAsync(user, password);
}