// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Review.InfrastructureLayer.Database;

public class ReviewConfiguration : IEntityTypeConfiguration<DomainModels.Review>
{
    public void Configure(EntityTypeBuilder<DomainModels.Review> entity)
    {
        entity.ToTable("reviews");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("review_id");
        entity.Property(e => e.ProductId).HasColumnName("product_id");
        entity.HasOne<DomainModels.Review>()
            .WithMany()
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        entity.Property(e => e.Rating).HasColumnName("review_rating");
        entity.Property(e => e.Text).HasColumnName("review_text");
        entity.Property(e => e.CreatedAt).HasColumnName("review_created_at");
        entity.Property(e => e.UpdatedAt).HasColumnName("review_updated_at");
    }
}
