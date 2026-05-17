using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomFinale.DataAccess.Entities;

[Table("Products")]
public class Product : AuditEntity
{
    [Key]
    public int Id {get; set;}

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    [MaxLength(1200)]
    public string Description { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal Price { get; set;}

    [Range(0, int.MaxValue)]
    public int StockQuantity {get; set;}

    public int ProductCategoryId {get; set;}

    [ForeignKey(nameof(ProductCategoryId))]
    public ProductCategory ProductCategory { get; set; }

    public ICollection<CartItem> CartItems {get; set;}

    public ICollection<OrderItem> OrderItems {get; set;}
}
