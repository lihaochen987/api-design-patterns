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
            .OwnsOne(p => p.DiscountPercentage);
        modelBuilder.Entity<Product.DomainModels.Product>()
            .OwnsOne(p => p.TaxRate);

        base.OnModelCreating(modelBuilder);
    }
}