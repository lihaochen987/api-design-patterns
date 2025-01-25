// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using System.Text;
using backend.Review.Services;
using backend.Shared;
using Dapper;

namespace backend.Review.InfrastructureLayer.Database.ReviewView;

public class ReviewViewRepository(
    IDbConnection dbConnection,
    ReviewSqlFilterBuilder reviewSqlFilterBuilder,
    QueryService<DomainModels.ReviewView> queryService)
    : IReviewViewRepository
{
    public async Task<DomainModels.ReviewView?> GetReviewView(long id)
    {
        return await dbConnection.QuerySingleOrDefaultAsync<DomainModels.ReviewView>(ReviewViewQueries.GetReviewView,
            new { Id = id });
    }

    public async Task<(List<DomainModels.ReviewView>, string?)> ListReviewsAsync(
        string? pageToken,
        string? filter,
        int maxPageSize,
        string? parent)
    {
        var sql = new StringBuilder(ReviewViewQueries.ListReviewsBase);
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
            string filterClause = reviewSqlFilterBuilder.BuildSqlWhereClause(filter);
            sql.Append($" AND {filterClause}");
        }

        // Order and limit
        sql.Append(" ORDER BY review_id LIMIT @PageSizePlusOne");
        parameters.Add("PageSizePlusOne", maxPageSize + 1);

        List<DomainModels.ReviewView> reviews = (await dbConnection.QueryAsync<DomainModels.ReviewView>(sql.ToString(), parameters)).ToList();
        List<DomainModels.ReviewView> paginatedReviews = queryService.Paginate(reviews, maxPageSize, out string? nextPageToken);

        return (paginatedReviews, nextPageToken);
    }
}
