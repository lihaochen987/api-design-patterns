using Microsoft.EntityFrameworkCore;

namespace backend.Product.InfrastructureLayer.Database;

public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<DomainModels.Product> Products { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ProductViewConfiguration());
        modelBuilder.ApplyConfiguration(new ProductPricingViewConfiguration());
        modelBuilder.ApplyConfiguration(new PetFoodConfiguration());
        modelBuilder.ApplyConfiguration(new GroomingAndHygieneConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
