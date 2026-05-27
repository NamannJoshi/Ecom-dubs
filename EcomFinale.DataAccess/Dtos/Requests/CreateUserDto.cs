using System.ComponentModel.DataAnnotations;

#nullable disable
namespace EcomFinale.DataAccess.Dtos.Requests;

public class CreateUserDto
{
    [Required]
    [MaxLength(250)]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(300)]
    public string Email { get; set; }

    [Required]
    [MaxLength(250)]
    public string PasswordHash { get; set; }
}