using Microsoft.EntityFrameworkCore;

namespace backend.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product.DomainModels.Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product.DomainModels.Product>()
            .OwnsOne(p => p.Dimensions);

        base.OnModelCreating(modelBuilder);
    }
}