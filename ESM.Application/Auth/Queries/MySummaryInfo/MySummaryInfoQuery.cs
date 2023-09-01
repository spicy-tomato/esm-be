using AutoMapper;
using ESM.Application.Common.Exceptions.Core;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Domain.Entities;
using ESM.Domain.Identity;
using MediatR;

namespace ESM.Application.Auth.Queries.MySummaryInfo;

public record MySummaryInfoQuery : IRequest<Result<MySummaryInfoDto>>;

public class MySummaryInfoQueryHandler : IRequestHandler<MySummaryInfoQuery, Result<MySummaryInfoDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public MySummaryInfoQueryHandler(IMapper mapper,
        ICurrentUserService currentUserService,
        IIdentityService identityService,
        IApplicationDbContext context)
    {
        _mapper = mapper;
        _currentUserService = currentUserService;
        _identityService = identityService;
        _context = context;
    }

    public async Task<Result<MySummaryInfoDto>> Handle(MySummaryInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await GetUser();
        var roles = await _identityService.GetRolesAsync(user);
        var teacher = GetTeacher(user.Id);

        var result = BindData(user, teacher, roles);

        return Result<MySummaryInfoDto>.Get(result);
    }

    private async Task<ApplicationUser> GetUser()
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

        return user;
    }

    private Teacher? GetTeacher(Guid userId) => _context.Teachers.FirstOrDefault(t => t.Id == userId);

    private MySummaryInfoDto BindData(ApplicationUser user, Teacher? teacher, IList<string> roles)
    {
        if (teacher is null)
        {
            return new MySummaryInfoDto
            {
                Id = user.Id,
                Roles = roles,
                PhoneNumber = user.PhoneNumber
            };
        }

        var result = _mapper.Map<MySummaryInfoDto>(teacher);

        result.Id = user.Id;
        result.Roles = roles;
        result.PhoneNumber = user.PhoneNumber;

        return result;
    }
}