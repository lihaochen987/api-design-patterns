using backend.Product;
using Microsoft.EntityFrameworkCore;

namespace backend.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product.Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define primary field for product
        modelBuilder.Entity<Product.Product>().HasKey(p => p.Id);

        // Storing ProductCategory as a string
        modelBuilder.Entity<Product.Product>()
            .Property(p => p.Category)
            .HasConversion<string>();

        modelBuilder.Entity<Product.Product>().HasData(
            new Product.Product(1, "Dry Dog Food", 50m, Category.DogFood),
            new Product.Product(2, "Wet Dog Food", 35m, Category.DogFood),
            new Product.Product(3, "Dog Treats", 10m, Category.DogFood),
            new Product.Product(4, "Chew Toy", 15m, Category.Toys),
            new Product.Product(5, "Fetch Ball", 8m, Category.Toys),
            new Product.Product(6, "Dog Collar", 12m, Category.CollarsAndLeashes),
            new Product.Product(7, "Dog Leash", 20m, Category.CollarsAndLeashes),
            new Product.Product(8, "Dog Shampoo", 10m, Category.GroomingAndHygiene),
            new Product.Product(9, "Dog Brush", 7m, Category.GroomingAndHygiene),
            new Product.Product(10, "Comfort Dog Bed", 80m, Category.Beds)
        );
    }
}