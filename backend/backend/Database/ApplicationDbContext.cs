using backend.Product;
using backend.Product.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace backend.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product.DomainModels.Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define primary field for product
        modelBuilder.Entity<Product.DomainModels.Product>().HasKey(p => p.Id);

        // Storing ProductCategory as a string
        modelBuilder.Entity<Product.DomainModels.Product>()
            .Property(p => p.Category)
            .HasConversion<string>();

        modelBuilder.Entity<Product.DomainModels.Product>().OwnsOne(p => p.Dimensions, d =>
        {
            d.Property(dim => dim.Length).HasColumnName("Length");
            d.Property(dim => dim.Width).HasColumnName("Width");
            d.Property(dim => dim.Height).HasColumnName("Height");
        });
    }

    public void Seed()
    {
        // Uncomment the below to force a re-seed
        Products.RemoveRange(Products);
        SaveChanges();
        
        if (Products.Any()) return;
        Products.AddRange(
            new Product.DomainModels.Product(1, "Dry Dog Food", 50m, Category.DogFood, new Dimensions(10m, 5m, 3m)),
            new Product.DomainModels.Product(2, "Wet Dog Food", 35m, Category.DogFood, new Dimensions(8m, 4m, 3m)),
            new Product.DomainModels.Product(3, "Dog Treats", 10m, Category.DogFood, new Dimensions(5m, 3m, 1m)),
            new Product.DomainModels.Product(4, "Chew Toy", 15m, Category.Toys, new Dimensions(6m, 6m, 4m)),
            new Product.DomainModels.Product(5, "Fetch Ball", 8m, Category.Toys, new Dimensions(4m, 4m, 4m)),
            new Product.DomainModels.Product(6, "Dog Collar", 12m, Category.CollarsAndLeashes,
                new Dimensions(5m, 1m, 0.5m)),
            new Product.DomainModels.Product(7, "Dog Leash", 20m, Category.CollarsAndLeashes,
                new Dimensions(120m, 2m, 0.5m)),
            new Product.DomainModels.Product(8, "Dog Shampoo", 10m, Category.GroomingAndHygiene,
                new Dimensions(8m, 4m, 2m)),
            new Product.DomainModels.Product(9, "Dog Brush", 7m, Category.GroomingAndHygiene,
                new Dimensions(7m, 3m, 2m)),
            new Product.DomainModels.Product(10, "Comfort Dog Bed", 80m, Category.Beds, new Dimensions(60m, 40m, 10m))
        );
        SaveChanges();
    }
}