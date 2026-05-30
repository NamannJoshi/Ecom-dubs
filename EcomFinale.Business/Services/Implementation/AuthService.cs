using BCrypt.Net;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Dtos.Requests;
using EcomFinale.DataAccess.Entities;

namespace EcomFinale.Business.Services.Implementation;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;

    public AuthService(ITokenService tokenService, IUserService userService)
    {
        _tokenService = tokenService;
        _userService = userService;
    }

    public async Task<TokenResponseDto> Login(LoginDto request)
    {
        var user = await _userService.GetByEmail(request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var accessToken = _tokenService.GenerateToken(new Models.JwtClaims
        {
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role,
        });
        var refreshToken = _tokenService.GenerateRefreshToken();

        return new TokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<UserDto> Register(RegisterDto request)
    {
        var existingUser = await _userService.GetByEmail(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var newUser = new CreateUserDto
        {
            Email = request.Email,
            PasswordHash = hashedPassword,
            Username = request.Username,
        };

        var createdUser = await _userService.Create(newUser);
        return createdUser;
    }

    public async Task<string> CheckRefreshToken(string refreshToken)
    {
        var entity = await _tokenService.GetRefreshTokenAsync(refreshToken);

        if (entity == null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }

        if (entity.ExpiresAt < DateTime.UtcNow || entity.IsRevoked)
        {
            throw new UnauthorizedAccessException("Refresh token has expired or been revoked.");
        }

        var user = await _userService.GetById(entity.UserId);
        if (user == null)
        {
            throw new UnauthorizedAccessException("User associated with the refresh token not found.");
        }

        var token =_tokenService.GenerateToken(new Models.JwtClaims
        {
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role,
        });
        return token;
    }
}