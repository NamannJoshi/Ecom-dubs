using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
namespace EcomFinale.DataAccess.Entities;

[Table("Users")]
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

    public ICollection<Order> Orders {get; set;}

    public ICollection<Cart> Carts {get;set;}
}
