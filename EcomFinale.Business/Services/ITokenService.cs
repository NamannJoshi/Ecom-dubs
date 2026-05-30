using EcomFinale.Business.Models;
using EcomFinale.DataAccess.Entities;
using EcomFinale.DataAccess.Options;
using Microsoft.Extensions.Options;

namespace EcomFinale.Business.Services;

public interface ITokenService
{
    string GenerateToken(JwtClaims userClaims);

    string GenerateRefreshToken();

    Task<RefreshToken?> GetRefreshTokenAsync(string token);

    Task RevokeRefreshTokenAsync(string token);
}