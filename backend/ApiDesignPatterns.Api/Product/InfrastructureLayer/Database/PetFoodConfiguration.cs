using System.Text.Json;
using backend.Product.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Product.InfrastructureLayer.Database;

public class PetFoodConfiguration : IEntityTypeConfiguration<PetFood>
{
    public void Configure(EntityTypeBuilder<PetFood> entity)
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
        entity.OwnsOne(e => e.WeightKg,
            weightKg => { weightKg.Property(d => d.Value).HasColumnName("product_pet_foods_weight_kg"); });
    }
}
