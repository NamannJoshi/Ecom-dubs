using System;
using EcomFinale.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcomFinale.DataAccess.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasOne(p => p.ProductCategory)
            .WithMany(pc => pc.Products)
            .HasForeignKey(p => p.ProductCategoryId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(p => p.CreatedByUser)
            .WithMany()
            .HasForeignKey(p => p.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(p => p.ModifiedByUser)
            .WithMany()
            .HasForeignKey(p => p.ModifiedBy)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
