using backend.Review.DomainModels.Views;
using Microsoft.EntityFrameworkCore;

namespace backend.Review.InfrastructureLayer.Database;

public class ReviewDbContext(DbContextOptions<ReviewDbContext> options) : DbContext(options)
{
    public DbSet<DomainModels.Review> Reviews { get; init; }
    public DbSet<ReviewView> ReviewViews { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewViewConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
