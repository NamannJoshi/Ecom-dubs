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
        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<ActionResult<string>> Register([FromBody] RegisterDto request)
    {
        var token = await authService.Register(request);
        return Ok(new { Token = token });
    }
}