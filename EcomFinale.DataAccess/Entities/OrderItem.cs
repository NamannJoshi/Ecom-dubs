using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomFinale.DataAccess.Entities;

[Table("OrderItems")]
public class OrderItem
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int OrderId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal UnitPrice { get; set; }

    public DateTime CreatedAt {get; set;}

    public DateTime ModifiedAt {get; set;}

    [ForeignKey(nameof(ProductId))]
    public Product Product {get; set;}

    [ForeignKey(nameof(OrderId))]
    public Order Order {get; set;}
}
