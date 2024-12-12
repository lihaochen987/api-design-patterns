using backend.Product.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Product.InfrastructureLayer.Database;

public class GroomingAndHygieneConfiguration : IEntityTypeConfiguration<GroomingAndHygiene>
{
    public void Configure(EntityTypeBuilder<GroomingAndHygiene> entity)
    {
        entity.ToTable("product_grooming_and_hygiene");

        entity.HasOne<DomainModels.Product>()
            .WithOne()
            .HasForeignKey<GroomingAndHygiene>(p => p.Id)
            .OnDelete(DeleteBehavior.Cascade);

        entity.Property(p => p.IsNatural)
            .HasColumnName("product_grooming_and_hygiene_is_natural");

        entity.Property(p => p.IsHypoallergenic)
            .HasColumnName("product_grooming_and_hygiene_is_hypoallergenic");

        entity.Property(p => p.UsageInstructions)
            .HasColumnName("product_grooming_and_hygiene_usage_instructions");

        entity.Property(p => p.IsCrueltyFree)
            .HasColumnName("product_grooming_and_hygiene_is_cruelty_free");

        entity.Property(p => p.SafetyWarnings)
            .HasColumnName("product_grooming_and_hygiene_safety_warnings");
    }
}
