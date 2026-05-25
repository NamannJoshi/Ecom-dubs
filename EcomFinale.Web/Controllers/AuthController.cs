using EcomFinale.Business.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace EcomFinale.Web.Controllers;

[ApiController]
[Route("api/Auth")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // For demonstration, we are using hardcoded user validation.
        // In a real application, you would validate the user against a database.
        if (request.Email == "user@example.com" && request.Password == "password")
        {
            var token = _tokenService.GenerateToken("1", request.Email);
            return Ok(new { Token = token });
        }

        return Unauthorized();
    }
}