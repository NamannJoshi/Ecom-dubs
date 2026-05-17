using System;
using EcomFinale.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcomFinale.DataAccess.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(o => o.CreatedByUser)
            .WithMany()
            .HasForeignKey(o => o.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(o => o.ModifiedByUser)
            .WithMany()
            .HasForeignKey(o => o.ModifiedBy)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
