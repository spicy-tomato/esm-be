using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Interfaces;
using ESM.Domain.Identity;

namespace ESM.Presentation.Services;

public class UserService : IUserService
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public UserService(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public bool UserExist(Guid id) => _context.Users.FirstOrDefault(u => u.Id == id) != null;

    public bool UserExist(string id) => Guid.TryParse(id, out var guid) && UserExist(guid);

    public async Task<ApplicationUser> CheckIfExistAndReturnEntity(string id)
    {
        var user = await _identityService.FindUserByIdAsync(id);
        if (user is null)
        {
            throw new NotFoundException(nameof(ApplicationUser), id);
        }

        return user;
    }
}