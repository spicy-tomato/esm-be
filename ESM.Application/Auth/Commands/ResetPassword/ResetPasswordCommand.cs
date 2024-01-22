using ESM.Application.Common.Exceptions.Core;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Application.Users.Exceptions;
using MediatR;

namespace ESM.Application.Auth.Commands.ResetPassword;

public record ResetPasswordCommand(string UserId) : IRequest<Result<bool>>;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<bool>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;

    public ResetPasswordCommandHandler(IIdentityService identityService, ICurrentUserService currentUserService)
    {
        _identityService = identityService;
        _currentUserService = currentUserService;
    }

    public async Task<Result<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        if (currentUserId is null)
        {
            throw new UnauthorizedException();
        }

        var currentUser = await _identityService.FindUserByIdAsync(currentUserId);
        if (currentUser is not { UserName: "Admin" })
        {
            throw new UnauthorizedException("User ID does not exist or doesn't have permission");
        }

        var user = await _identityService.FindUserByIdAsync(request.UserId);
        if (user is null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        var token = await _identityService.GeneratePasswordResetTokenAsync(user);

        var result = await _identityService.ResetPasswordAsync(user, token, "e10adc3949ba59abbe56e057f20f883e");

        return result;
    }
}