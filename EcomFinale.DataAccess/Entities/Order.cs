using System.ComponentModel.DataAnnotations.Schema;

namespace EcomFinale.DataAccess.Entities;

[Table("Orders")]
public class Order : AuditEntity
{
    public int Id { get; set; }

    public int UserId {get; set;}

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
