using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EcomFinale.DataAccess.Entities;

[Table("Orders")]
[Index(nameof(IdempotencyId), IsUnique = true)]
public class Order : AuditEntity
{
    public int Id { get; set; }

    public int UserId {get; set;}

    public Guid IdempotencyId { get; set;}

    public string? PaymentId { get; set; }

    public PaymentMethod? PaymentMethod { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    public OrderStatus Status { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User {get; set;}

    public ICollection<Order> Orders { get; set; }

    public ICollection<Payment> Payments {get; set;}

    public ICollection<OrderItem> OrderItems {get; set;}
}

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled,
    Refunded,
}

public enum PaymentMethod
{
    CreditCard,
    PayPal,
    BankTransfer,
    CashOnDelivery,
}
