// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using System.Text;
using backend.Review.DomainModels;
using backend.Review.Services;
using Dapper;

namespace backend.Review.InfrastructureLayer;

public class ReviewViewRepository(
    IDbConnection dbConnection,
    SqlFilterBuilder sqlFilterBuilder)
    : IReviewViewRepository
{
    public async Task<ReviewView?> GetReviewView(long id)
    {
        const string query = """
                                 SELECT
                                     review_id AS Id,
                                     product_id AS ProductId,
                                     review_rating AS Rating,
                                     review_text AS Text,
                                     review_created_at AS CreatedAt,
                                     review_updated_at AS UpdatedAt
                                 FROM reviews_view
                                 WHERE review_id = @Id
                             """;

        return await dbConnection.QuerySingleOrDefaultAsync<ReviewView>(query, new { Id = id });
    }

    public async Task<(List<ReviewView>, string?)> ListReviewsAsync(
        string? pageToken,
        string? filter,
        int maxPageSize,
        string? parent)
    {
        var sql = new StringBuilder("""
                                        SELECT
                                            review_id AS Id,
                                            product_id AS ProductId,
                                            review_rating AS Rating,
                                            review_text AS Text,
                                            review_created_at AS CreatedAt,
                                            review_updated_at AS UpdatedAt
                                        FROM reviews_view
                                        WHERE 1=1
                                    """);

        var parameters = new DynamicParameters();

        // Parent filter
        if (!string.IsNullOrWhiteSpace(parent) && long.TryParse(parent, out long parentId))
        {
            sql.Append(" AND product_id = @ParentId");
            parameters.Add("ParentId", parentId);
        }

        // Pagination filter
        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenReview))
        {
            sql.Append(" AND review_id > @LastSeenReview");
            parameters.Add("LastSeenReview", lastSeenReview);
        }

        // Custom filter
        if (!string.IsNullOrEmpty(filter))
        {
            string filterClause = sqlFilterBuilder.BuildSqlWhereClause(filter);
            sql.Append($" AND {filterClause}");
        }

        // Order and limit
        sql.Append(" ORDER BY review_id LIMIT @PageSizePlusOne");
        parameters.Add("PageSizePlusOne", maxPageSize + 1);

        var reviews = (await dbConnection.QueryAsync<ReviewView>(sql.ToString(), parameters)).ToList();
        var paginatedReviews = Paginate(reviews, maxPageSize, out string? nextPageToken);

        return (paginatedReviews, nextPageToken);
    }

    private static List<ReviewView> Paginate(
        List<ReviewView> existingItems,
        int maxPageSize,
        out string? nextPageToken)
    {
        if (existingItems.Count <= maxPageSize)
        {
            nextPageToken = null;
            return existingItems;
        }

        ReviewView lastItemInPage = existingItems[maxPageSize - 1];
        nextPageToken = lastItemInPage.Id.ToString();
        return existingItems.Take(maxPageSize).ToList();
    }
}
