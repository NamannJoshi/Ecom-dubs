using EcomFinale.DataAccess.Entities;

namespace EcomFinale.DataAccess.Dtos;

public class CartDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public CartStatus CartStatus {get; set;} = CartStatus.Active;

    public ICollection<CartItemDto> CartItems { get; set; }
}