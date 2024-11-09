using backend.Product.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace backend.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product.DomainModels.Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        const string connectionString = "Host=postgres;Database=mydatabase;Username=myusername;Password=mypassword";
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define primary field for product
        modelBuilder.Entity<Product.DomainModels.Product>().HasKey(p => p.Id);

        // Storing ProductCategory as a string
        modelBuilder.Entity<Product.DomainModels.Product>()
            .Property(p => p.Category)
            .HasConversion<string>();

        modelBuilder.Entity<Product.DomainModels.Product>().OwnsOne(p => p.Dimensions);
    }
}