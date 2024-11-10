using Microsoft.EntityFrameworkCore;

namespace backend.Product.Database;

public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<DomainModels.Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DomainModels.Product>(entity =>
        {
            entity.ToTable("products");

            entity.Property(e => e.Id)
                .HasColumnName("product_id");
            entity.Property(e => e.Name)
                .HasColumnName("product_name");
            entity.Property(e => e.BasePrice)
                .HasColumnName("product_base_price");
            entity.Property(e => e.Category)
                .HasColumnName("product_category")
                .HasConversion<string>();

            entity.OwnsOne(p => p.DiscountPercentage,
                dp => { dp.Property(d => d.Value).HasColumnName("product_discount_percentage"); });
            entity.OwnsOne(p => p.TaxRate,
                tr => { tr.Property(t => t.Value).HasColumnName("product_tax_rate"); });
            entity.OwnsOne(e => e.Dimensions, dimensions =>
            {
                dimensions.Property(d => d.Length)
                    .HasColumnName("product_dimensions_length");
                dimensions.Property(d => d.Width)
                    .HasColumnName("product_dimensions_width");
                dimensions.Property(d => d.Height)
                    .HasColumnName("product_dimensions_height");
            });

            entity.Property(e => e.Price)
                .UsePropertyAccessMode(PropertyAccessMode.Property);
        });

        base.OnModelCreating(modelBuilder);
    }
}