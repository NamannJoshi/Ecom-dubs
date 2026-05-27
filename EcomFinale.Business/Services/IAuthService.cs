using EcomFinale.DataAccess.Dtos;

namespace EcomFinale.Business.Services;

public interface IAuthService
{
    Task<string> Login(LoginDto request);

    Task<string> Register(RegisterDto request);
}