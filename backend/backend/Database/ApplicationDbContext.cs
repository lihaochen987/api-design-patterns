using Microsoft.EntityFrameworkCore;

namespace backend.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product.DomainModels.Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product.DomainModels.Product>()
            .OwnsOne(p => p.Dimensions);

        modelBuilder.Entity<Product.DomainModels.Product>()
            .OwnsOne(p => p.DiscountPercentage, dp => { dp.Property(d => d.Value); });

        modelBuilder.Entity<Product.DomainModels.Product>()
            .OwnsOne(p => p.TaxRate, tr => { tr.Property(t => t.Value); });

        modelBuilder.Entity<Product.DomainModels.Product>()
            .Property(p => p.Category)
            .HasConversion<string>();

        base.OnModelCreating(modelBuilder);
    }
}