using backend.Product.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace backend.Product.Database;

public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<DomainModels.BaseProduct> Products { get; init; }

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


        modelBuilder.Entity<DomainModels.BaseProduct>(entity =>
        {
            entity.ToTable("products");

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

            // product_category
            entity.Property(e => e.Category)
                .HasColumnName("product_category")
                .HasConversion<int>()
                .HasColumnType("bigint");

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

        base.OnModelCreating(modelBuilder);
    }
}