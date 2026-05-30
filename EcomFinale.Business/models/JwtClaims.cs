using EcomFinale.DataAccess.Entities;

namespace EcomFinale.Business.Models;

public class JwtClaims
{
    public int UserId { get; set; }

    public string Email { get; set ;}

    public UserRole Role { get; set; }
}