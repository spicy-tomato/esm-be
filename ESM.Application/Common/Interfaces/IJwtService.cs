using ESM.Domain.Dtos;
using ESM.Domain.Identity;

namespace ESM.Application.Common.Interfaces;

public interface IJwtService
{
    GeneratedToken CreateToken(ApplicationUser user);
}