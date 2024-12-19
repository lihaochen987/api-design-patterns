// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Product.InfrastructureLayer.Database;

public class ProductConfiguration : IEntityTypeConfiguration<Product.DomainModels.Product>
{
    public void Configure(EntityTypeBuilder<Product.DomainModels.Product> entity)
    {
        entity.ToTable("products");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("product_id");
        entity.Property(e => e.Name).HasColumnName("product_name");
        entity.OwnsOne(e => e.Dimensions, dimensions =>
        {
            dimensions.Property(d => d.Length).HasColumnName("product_dimensions_length_cm");
            dimensions.Property(d => d.Width).HasColumnName("product_dimensions_width_cm");
            dimensions.Property(d => d.Height).HasColumnName("product_dimensions_height_cm");
        });
        entity.OwnsOne(e => e.Pricing, pricing =>
        {
            pricing.Property(p => p.BasePrice).HasColumnName("product_base_price");
            pricing.Property(p => p.DiscountPercentage).HasColumnName("product_discount_percentage");
            pricing.Property(p => p.TaxRate).HasColumnName("product_tax_rate");
        });
        entity.Property(e => e.Category)
            .HasColumnName("product_category")
            .HasConversion<int>();
    }
}
