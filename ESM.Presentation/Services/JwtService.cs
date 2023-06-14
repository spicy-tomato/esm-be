using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ESM.Application.Common.Interfaces;
using ESM.Data.Dtos;
using ESM.Data.Models;
using ESM.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace ESM.API.Services;

public class JwtService : IJwtService
{
    private const int EXPIRATION_MINUTES = 60 * 24 * 10; // 10 days
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public GeneratedToken CreateToken(User user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(EXPIRATION_MINUTES);
        var token = CreateJwtToken(CreateClaims(user), CreateSigningCredentials(), expiration);
        var tokenHandler = new JwtSecurityTokenHandler();

        return new GeneratedToken
        {
            Token = tokenHandler.WriteToken(token),
            // RefreshToken = CreateRefreshToken(),
            Expiration = expiration
        };
    }

    private JwtSecurityToken CreateJwtToken(IEnumerable<Claim> claims,
        SigningCredentials credentials,
        DateTime expiration) =>
        new(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expiration,
            signingCredentials: credentials);

    private static IEnumerable<Claim> CreateClaims(User user) => new[]
    {
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName)
    };

    private SigningCredentials CreateSigningCredentials() => new(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty)),
        SecurityAlgorithms.HmacSha256);
}