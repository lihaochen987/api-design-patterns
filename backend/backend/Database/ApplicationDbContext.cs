using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define primary field for product
        modelBuilder.Entity<Product>().HasKey(p => p.ProductId);

        // Storing ProductCategory as a string
        modelBuilder.Entity<Product>()
            .Property(p => p.ProductCategory)
            .HasConversion<string>();

        modelBuilder.Entity<Product>().HasData(
            new Product(1, "Dry Dog Food", 50m, ProductCategory.DogFood),
            new Product(2, "Wet Dog Food", 35m, ProductCategory.DogFood),
            new Product(3, "Dog Treats", 10m, ProductCategory.DogFood),
            new Product(4, "Chew Toy", 15m, ProductCategory.Toys),
            new Product(5, "Fetch Ball", 8m, ProductCategory.Toys),
            new Product(6, "Dog Collar", 12m, ProductCategory.CollarsAndLeashes),
            new Product(7, "Dog Leash", 20m, ProductCategory.CollarsAndLeashes),
            new Product(8, "Dog Shampoo", 10m, ProductCategory.GroomingAndHygiene),
            new Product(9, "Dog Brush", 7m, ProductCategory.GroomingAndHygiene),
            new Product(10, "Comfort Dog Bed", 80m, ProductCategory.Beds)
        );
    }
}