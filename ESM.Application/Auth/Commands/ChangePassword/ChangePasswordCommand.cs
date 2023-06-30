using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using MediatR;

namespace ESM.Application.Auth.Commands.ChangePassword;

public record ChangePasswordCommand(string OldPassword, string NewPassword) : IRequest<Result<bool>>;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<bool>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;

    public ChangePasswordCommandHandler(ICurrentUserService currentUserService, IIdentityService identityService)
    {
        _currentUserService = currentUserService;
        _identityService = identityService;
    }

    public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId is null ||
            await _identityService.GetUserNameAsync(userId) is null)
        {
            throw new UnauthorizedException();
        }

        var result = await _identityService.ChangePasswordAsync(userId, request.OldPassword, request.NewPassword);

        return result;
    }
}