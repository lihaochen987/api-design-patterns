using backend.Product.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace backend.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product.DomainModels.Product> Products { get; set; }
    public DbSet<ProductReviewSummary> ProductReviewSummaries { get; set; }
}