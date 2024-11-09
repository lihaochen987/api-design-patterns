using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Product.DomainModels;

public class ProductReviewSummary
{
    [Column("product_id")] public long Id { get; private set; }

    [Column("product_rating_distribution")] public RatingDistribution RatingDistribution { get; private set; }

    [Column("product_last_review_date")] public DateOnly LastReviewDate { get; private set; }

    [Column("product_last_review_time")] public TimeOnly LastReviewTime { get; private set; }

    public int AverageRating { get; private set; }

    public int TotalReviews { get; private set; }
}