using backend.Review.DomainModels.Views;
using Microsoft.EntityFrameworkCore;

namespace backend.Review.InfrastructureLayer.Database;

public class ReviewDbContext(DbContextOptions<ReviewDbContext> options) : DbContext(options)
{
    public DbSet<DomainModels.Review> Reviews { get; init; }
    public DbSet<ReviewView> ReviewViews { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewView>(entity =>
        {
            entity.ToView("reviews_view");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("review_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Rating).HasColumnName("review_rating");
            entity.Property(e => e.Text).HasColumnName("review_text");
            entity.Property(e => e.CreatedAt).HasColumnName("review_created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("review_updated_at");
        });

        modelBuilder.Entity<DomainModels.Review>(entity =>
        {
            entity.ToTable("reviews");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("review_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.HasOne<DomainModels.Review>()
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.Rating).HasColumnName("review_rating");
            entity.Property(e => e.Text).HasColumnName("review_text");
            entity.Property(e => e.CreatedAt).HasColumnName("review_created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("review_updated_at");
        });
        base.OnModelCreating(modelBuilder);
    }
}
