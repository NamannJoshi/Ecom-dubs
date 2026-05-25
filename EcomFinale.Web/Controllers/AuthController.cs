using EcomFinale.Business.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace EcomFinale.Web.Controllers;

[ApiController]
[Route("api/Auth")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;

    public AuthController(ITokenService tokenService, IUserService userService)
    {
        _tokenService = tokenService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // For demonstration, we are using hardcoded user validation.
        // In a real application, you would validate the user against a database.
        var user = await _userService.GetByEmail(request.Email);
        if (user == null || user.PasswordHash != request.Password)
        {
            return Unauthorized();
        }
    
        var token = _tokenService.GenerateToken(user.Id.ToString(), request.Email, user.Role);
        return Ok(new { Token = token });
    }
}