using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EcomFinale.DataAccess.Entities;

[Table("RefreshTokens")]
[Index(nameof(Token), IsUnique = true)]
public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } // The random opaque string
    public int UserId { get; set; } // Foreign key to your User table
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
}