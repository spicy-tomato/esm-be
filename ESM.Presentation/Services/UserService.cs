using ESM.Application.Common.Interfaces;

namespace ESM.Presentation.Services;

public class UserService : IUserService
{
    private readonly IApplicationDbContext _context;

    public UserService(IApplicationDbContext context)
    {
        _context = context;
    }

    public bool UserExist(Guid id) => _context.Users.FirstOrDefault(u => u.Id == id) != null;

    public bool UserExist(string id) => Guid.TryParse(id, out var guid) && UserExist(guid);
}