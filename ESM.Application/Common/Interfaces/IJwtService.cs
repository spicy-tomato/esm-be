using ESM.Data.Dtos;

namespace ESM.Application.Common.Interfaces;

public interface IJwtService
{
    GeneratedToken CreateToken(IApplicationUser user);
}