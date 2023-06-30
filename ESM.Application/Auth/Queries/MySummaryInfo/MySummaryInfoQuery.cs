using AutoMapper;
using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Data.Dtos.User;
using MediatR;

namespace ESM.Application.Auth.Queries.MySummaryInfo;

public record MySummaryInfoQuery : IRequest<Result<UserSummary>>;

public class MySummaryInfoQueryHandler : IRequestHandler<MySummaryInfoQuery, Result<UserSummary>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public MySummaryInfoQueryHandler(IMapper mapper,
        ICurrentUserService currentUserService,
        IIdentityService identityService)
    {
        _mapper = mapper;
        _currentUserService = currentUserService;
        _identityService = identityService;
    }

    public async Task<Result<UserSummary>> Handle(MySummaryInfoQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId is null)
        {
            throw new UnauthorizedException();
        }

        var user = await _identityService.FindUserByIdAsync(userId);
        if (user is null)
        {
            throw new UnauthorizedException();
        }

        var result = _mapper.Map<UserSummary>(user);

        return Result<UserSummary>.Get(result);
    }
}