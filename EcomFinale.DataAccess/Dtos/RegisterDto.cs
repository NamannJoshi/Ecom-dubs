using System.ComponentModel.DataAnnotations;

namespace EcomFinale.DataAccess.Dtos;

public class RegisterDto
{
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string Username { get; set; } = string.Empty;
}