using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;

namespace EcomFinale.DataAccess.Entities;

[Table("Orders")]
public class Order : AuditEntity
{
    public int Id { get; set; }

    public int UserId {get; set;}

    [ForeignKey(nameof(UserId))]
    public User User {get; set;}

    public ICollection<Order> Orders { get; set; }

    public ICollection<Payment> Payments {get; set;}

    public ICollection<OrderItem> OrderItems {get; set;}
}
