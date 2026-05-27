using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Dtos.Requests;

namespace EcomFinale.Business.Services;

public interface IAuthService
{
    Task<TokenResponseDto> Login(LoginDto request);

    Task<UserDto> Register(RegisterDto request);

    Task<string> CheckRefreshToken(string refreshToken);
}