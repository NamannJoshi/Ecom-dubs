using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EcomFinale.DataAccess.Entities;
using EcomFinale.DataAccess.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EcomFinale.Business.Services.Implementation;

public class TokenService : ITokenService
{
    private readonly IOptions<JwtOptions> jwtOptions;

    public TokenService(IOptions<JwtOptions> jwtOptions)
    {
        this.jwtOptions = jwtOptions;
    }

    public string GenerateToken(string userId, string email, UserRole role)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Value.Issuer,
            audience: jwtOptions.Value.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(jwtOptions.Value.ExpiryMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}