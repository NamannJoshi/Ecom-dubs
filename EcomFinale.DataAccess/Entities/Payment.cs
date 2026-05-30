using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomFinale.DataAccess.Entities;

public enum PaymentStatus
{
    Pending = 0,

    Paid = 1,

    BillingInitiated = 2,

    Failed = 3,

    RefundInitiated = 4,

    Refunded = 5,
}

[Table("Payments")]
public class Payment : AuditEntity
{
    [Key]
    public int Id { get; set; }

    public string? PaymentId { get; set; }

    public string? PaymentMethod { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal AmountPaid { get; set; }

    public int OrderId { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; }
}
