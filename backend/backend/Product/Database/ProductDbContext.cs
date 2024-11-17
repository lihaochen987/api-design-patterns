using Microsoft.EntityFrameworkCore;

namespace backend.Product.Database;

public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<DomainModels.Product> Products { get; init; }
    public DbSet<DomainModels.Product> ProductsWithoutPricing { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var productEntity = modelBuilder.Entity<DomainModels.Product>();
        new ProductViewBuilder(productEntity)
            .MapToTable("products_view")
            .WithPrimaryKey()
            .WithName()
            .WithDimensions()
            .WithCategory()
            .WithPricing()
            .WithCalculatedPrice();
    }
}