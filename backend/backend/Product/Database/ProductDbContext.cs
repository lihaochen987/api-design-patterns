using Microsoft.EntityFrameworkCore;

namespace backend.Product.Database;

public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<DomainModels.Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DomainModels.Product>(entity =>
        {
            entity.ToTable("products_view");

            entity.Property(e => e.Id)
                .HasColumnName("product_id");
            entity.Property(e => e.Name)
                .HasColumnName("product_name");
            entity.OwnsOne(e => e.Dimensions, dimensions =>
            {
                dimensions.Property(d => d.Length)
                    .HasColumnName("product_dimensions_length");
                dimensions.Property(d => d.Width)
                    .HasColumnName("product_dimensions_width");
                dimensions.Property(d => d.Height)
                    .HasColumnName("product_dimensions_height");
            });
            entity.Property(e => e.Category)
                .HasColumnName("product_category_name")
                .HasConversion<string>();

            entity.OwnsOne(e => e.Pricing, pricing =>
            {
                pricing.Property(p => p.BasePrice)
                    .HasColumnName("product_base_price");

                pricing.OwnsOne(p => p.DiscountPercentage,
                    dp => { dp.Property(d => d.Value).HasColumnName("product_discount_percentage"); });

                pricing.OwnsOne(p => p.TaxRate, tr => { tr.Property(t => t.Value).HasColumnName("product_tax_rate"); });
            });
            entity.Property(p => p.Price)
                .HasColumnName("product_price");
        });

        base.OnModelCreating(modelBuilder);
    }
}