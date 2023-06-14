using ESM.Data.Dtos;
using ESM.Data.Models;
using ESM.Domain.Entities;

namespace ESM.Application.Common.Interfaces;

public interface IJwtService
{
    GeneratedToken CreateToken(User user);
}