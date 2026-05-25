using EcomFinale.DataAccess.Entities;

namespace EcomFinale.DataAccess.Dtos;
#nullable disable
public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }

    public string PasswordHash {get; set;}
}