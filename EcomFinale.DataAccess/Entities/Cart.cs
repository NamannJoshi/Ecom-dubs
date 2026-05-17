using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomFinale.DataAccess.Entities;

[Table("Carts")]
public class Cart
{
    public int Id { get; set; }

    public int UserId { get; set;}

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }

    public DateTime? CheckedOutAtUtc { get; set; }

    public CartStatus CartStatus {get; set;} = CartStatus.Active;

    public User User {get; set;}

    public ICollection<CartItem> CartItems { get; set; }
}

public enum CartStatus
{
    Converted = 0,
    Active = 1,
    Abandoned = 2,
}
