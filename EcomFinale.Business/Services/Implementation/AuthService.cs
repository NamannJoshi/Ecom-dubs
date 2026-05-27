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

    public async Task<string> Login(LoginDto request)
    {
        var user = await _userService.GetByEmail(request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        return _tokenService.GenerateToken(user.Id.ToString(), request.Email, user.Role);
    }

    public async Task<string> Register(RegisterDto request)
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
        return _tokenService.GenerateToken(createdUser.Id.ToString(), createdUser.Email, createdUser.Role);
    }
}