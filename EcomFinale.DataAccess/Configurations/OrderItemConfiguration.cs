using System;
using EcomFinale.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcomFinale.DataAccess.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasOne(ci => ci.Order)
            .WithMany(c => c.OrderItems)
            .HasForeignKey(ci => ci.OrderId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(ci => ci.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
