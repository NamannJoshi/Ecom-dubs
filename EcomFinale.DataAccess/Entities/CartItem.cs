using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;

namespace EcomFinale.DataAccess.Entities;

[Table("CartItems")]
public class CartItem
{
    [Key]
    public int Id { get; set; }

    public int CartId {get; set;}

    public int ProductId {get; set;}

    public DateTime CreatedAt {get; set; }

    public DateTime ModifiedAt {get; set;}

    [Column(TypeName = "decimal(12,2)")]
    public decimal UnitPrice {get; set;}

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }

    [ForeignKey(nameof(CartId))]
    public Cart Cart {get; set;}
}
