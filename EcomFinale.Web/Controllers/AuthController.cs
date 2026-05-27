using EcomFinale.Business.Services;
using EcomFinale.DataAccess.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EcomFinale.Web.Controllers;

[ApiController]
[Route("api/Auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginDto request)
    {
        var token = await authService.Login(request);
        this.CreateCookie(token.RefreshToken);
        
        return Ok(new { Token = token.AccessToken });
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto request)
    {
        var user = await authService.Register(request);
        return Ok(user);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<string>> RefreshToken()
    {
        var cookie = Request.Cookies["RefreshToken"];
        if (string.IsNullOrEmpty(cookie))        {
            return BadRequest("Refresh token is missing.");
        }

        var token = await authService.CheckRefreshToken(cookie);
        return Ok(new { Token = token });
    }

    private void CreateCookie(string token)
    {
        var cookie = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Path = "/auth/refresh-token",
            Expires = DateTime.UtcNow.AddDays(7)
        };
        
        Response.Cookies.Append("RefreshToken", token, cookie);
    }
}