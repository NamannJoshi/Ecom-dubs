using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EcomFinale.Business.Models;
using EcomFinale.DataAccess.Entities;
using EcomFinale.DataAccess.Options;
using EcomFinale.DataAccess.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EcomFinale.Business.Services.Implementation;

public class TokenService : ITokenService
{
    private readonly IRefreshTokenRepository refreshTokenRepository;
    private readonly IOptions<JwtOptions> jwtOptions;

    public TokenService(IRefreshTokenRepository refreshTokenRepository, IOptions<JwtOptions> jwtOptions)
    {
        this.refreshTokenRepository = refreshTokenRepository;
        this.jwtOptions = jwtOptions;
    }

    public string GenerateToken(JwtClaims userClaims)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userClaims.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, userClaims.Email),
            new Claim(ClaimTypes.Role, userClaims.Role.ToString()),
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

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }

    public async Task SaveRefreshTokenAsync(string token, int userId)
    {
        var refreshToken = new RefreshToken
        {
            Token = token,
            UserId = userId,
            ExpiresAt = DateTime.Now.AddDays(7)
        };

        await refreshTokenRepository.AddAsync(refreshToken);
    }

    public async Task RevokeRefreshTokenAsync(string token)
    {
        await refreshTokenRepository.RevokeAsync(token);
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        return await refreshTokenRepository.GetByTokenAsync(token);
    }
}