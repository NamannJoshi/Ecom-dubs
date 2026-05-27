using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable
namespace EcomFinale.DataAccess.Entities;

[Table("Users")]
[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Key]
    public int Id {get; set;}

    [Required]
    [MaxLength(250)]
    public string Username {get; set;}

    [Required]
    [MaxLength(300)]
    public string Email {get; set;}

    [Required]
    public string PasswordHash {get; set;}

    public UserRole Role {get; set;} = UserRole.User;

    public ICollection<Order> Orders {get; set;}

    public ICollection<Cart> Carts {get;set;}
}

public enum UserRole
{
    Admin,
    User
}
