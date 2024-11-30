using System.Text.Json;
using backend.Product.DomainModels;
using backend.Product.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace backend.Product.Database;

public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<DomainModels.Product> Products { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductView>(entity =>
        {
            entity.ToView("products_view");

            // product_id PK
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("product_id");

            // product_name
            entity.Property(e => e.Name)
                .HasColumnName("product_name");

            entity.OwnsOne(e => e.Dimensions, dimensions =>
            {
                // product_dimensions_length
                dimensions.Property(d => d.Length)
                    .HasColumnName("product_dimensions_length_cm");

                // product_dimensions.width
                dimensions.Property(d => d.Width)
                    .HasColumnName("product_dimensions_width_cm");

                // product_dimensions_height
                dimensions.Property(d => d.Height)
                    .HasColumnName("product_dimensions_height_cm");
            });

            // product_category_name
            entity.Property(e => e.Category)
                .HasColumnName("product_category_name")
                .HasConversion<string>();

            // product_price
            entity.Property(p => p.Price)
                .HasColumnName("product_price");

            // PetFood columns
            entity.Property(p => p.AgeGroup)
                .HasColumnName("product_pet_food_age_group")
                .HasConversion<string>();

            entity.Property(p => p.BreedSize)
                .HasColumnName("product_pet_food_breed_size")
                .HasConversion<string>();

            entity.Property(p => p.Ingredients)
                .HasColumnName("product_pet_foods_ingredients");

            entity.Property(p => p.NutritionalInfo)
                .HasColumnName("product_pet_foods_nutritional_info")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = false }),
                    v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, new JsonSerializerOptions())!);

            entity.Property(p => p.StorageInstructions)
                .HasColumnName("product_pet_foods_storage_instructions");

            entity.Property(p => p.WeightKg)
                .HasColumnName("product_pet_foods_weight_kg");

            // Grooming and Hygiene columns
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
        });

        modelBuilder.Entity<ProductPricingView>(entity =>
        {
            entity.ToView("products_pricing_view");

            // product_id PK
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("product_id");

            entity.OwnsOne(e => e.Pricing, pricing =>
            {
                // Links product_id to pricing
                pricing.WithOwner();

                // product_base_price
                pricing.Property(p => p.BasePrice)
                    .HasColumnName("product_base_price");

                // product_discount_percentage
                pricing.Property(p => p.DiscountPercentage)
                    .HasColumnName("product_discount_percentage");

                // product_tax_rate
                pricing.Property(p => p.TaxRate)
                    .HasColumnName("product_tax_rate");
            });
        });

        modelBuilder.Entity<DomainModels.Product>(entity =>
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
        });

        modelBuilder.Entity<PetFood>(entity =>
        {
            entity.ToTable("product_pet_foods");

            entity.HasOne<DomainModels.Product>()
                .WithOne()
                .HasForeignKey<PetFood>(p => p.Id)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(p => p.AgeGroup)
                .HasColumnName("product_pet_foods_age_group_id")
                .HasConversion<int>();
            entity.Property(p => p.BreedSize)
                .HasColumnName("product_pet_foods_breed_size_id")
                .HasConversion<int>();
            entity.Property(p => p.Ingredients).HasColumnName("product_pet_foods_ingredients");
            entity.Property(p => p.NutritionalInfo)
                .HasColumnName("product_pet_foods_nutritional_info")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = false }),
                    v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, new JsonSerializerOptions())!);
            entity.Property(p => p.StorageInstructions).HasColumnName("product_pet_foods_storage_instructions");
            entity.Property(p => p.WeightKg).HasColumnName("product_pet_foods_weight_kg");
        });

        modelBuilder.Entity<GroomingAndHygiene>(entity =>
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
        });

        base.OnModelCreating(modelBuilder);
    }
}