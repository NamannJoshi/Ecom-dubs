using System;
using EcomFinale.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcomFinale.DataAccess.Configurations;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.HasOne(pc => pc.CreatedByUser)
            .WithMany()
            .HasForeignKey(pc => pc.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(pc => pc.ModifiedByUser)
            .WithMany()
            .HasForeignKey(pc => pc.ModifiedBy)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
