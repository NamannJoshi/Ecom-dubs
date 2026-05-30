using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EcomFinale.Business.Models;
using EcomFinale.DataAccess.Entities;
using Microsoft.AspNetCore.Http;

namespace EcomFinale.Business.Services.Implementation;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public JwtClaims? GetCurrentUserClaims()
    {
        var claims = this.httpContextAccessor.HttpContext?.User?.Claims;

        if (claims == null || !claims.Any())
        {
            return null;
        }

        var userId = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value ?? string.Empty;
        var email = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value ?? string.Empty;
        var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;

        Enum.TryParse<UserRole>(role, true, out var parsedRole);

        return new JwtClaims
        {
            UserId = userId,
            Email = email,
            Role = parsedRole,
        };
    }
}