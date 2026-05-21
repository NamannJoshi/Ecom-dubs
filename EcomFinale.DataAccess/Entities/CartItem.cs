using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomFinale.DataAccess.Entities;

[Table("CartItems")]
public class CartItem
{
    [Key]
    public int Id { get; set; }

    public int CartId {get; set;}

    public int ProductId {get; set;}

    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity {get; set;}

    public DateTime CreatedAt {get; set; }

    public DateTime ModifiedAt {get; set;}

    [Column(TypeName = "decimal(12,2)")]
    public decimal UnitPrice {get; set;}

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }

    [ForeignKey(nameof(CartId))]
    public Cart Cart {get; set;}
}
