namespace backend.Review.InfrastructureLayer.Queries;

public static class ReviewViewQueries
{
    public const string GetReviewView = """
                                        SELECT
                                            review_id AS Id,
                                            product_id AS ProductId,
                                            review_rating AS Rating,
                                            review_text AS Text,
                                            review_created_at AS CreatedAt,
                                            review_updated_at AS UpdatedAt
                                        FROM reviews_view
                                        WHERE review_id = @Id;
                                        """;

    public const string ListReviewsBase = """
                                          SELECT
                                              review_id AS Id,
                                              product_id AS ProductId,
                                              review_rating AS Rating,
                                              review_text AS Text,
                                              review_created_at AS CreatedAt,
                                              review_updated_at AS UpdatedAt
                                          FROM reviews_view
                                          WHERE 1=1
                                          """;
}
