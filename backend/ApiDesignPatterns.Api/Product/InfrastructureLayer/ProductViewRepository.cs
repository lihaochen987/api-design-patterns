using System.Data;
using System.Text;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer.Queries;
using backend.Product.Services;
using backend.Shared;
using Dapper;

namespace backend.Product.InfrastructureLayer;

public class ProductViewRepository(
    IDbConnection dbConnection,
    QueryService<ProductView> queryService,
    ProductSqlFilterBuilder productSqlFilterBuilder)
    : IProductViewRepository
{
    public async Task<ProductView?> GetProductView(long id)
    {
        return await dbConnection.QuerySingleOrDefaultAsync<ProductView>(ProductViewQueries.GetProductView,
            new { Id = id });
    }

    public async Task<(List<ProductView>, string?)> ListProductsAsync(
        string? pageToken,
        string? filter,
        int maxPageSize)
    {
        var sql = new StringBuilder(ProductViewQueries.ListProductsBase);
        var parameters = new DynamicParameters();

        // Pagination filter
        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenReview))
        {
            sql.Append(" AND review_id > @LastSeenReview");
            parameters.Add("LastSeenReview", lastSeenReview);
        }

        // Custom filter
        if (!string.IsNullOrEmpty(filter))
        {
            string filterClause = productSqlFilterBuilder.BuildSqlWhereClause(filter);
            sql.Append($" AND {filterClause}");
        }

        // Order and limit
        sql.Append(" ORDER BY review_id LIMIT @PageSizePlusOne");
        parameters.Add("PageSizePlusOne", maxPageSize + 1);

        List<ProductView> products = (await dbConnection.QueryAsync<ProductView>(sql.ToString(), parameters)).ToList();
        List<ProductView> paginatedReviews = queryService.Paginate(products, maxPageSize, out string? nextPageToken);

        return (paginatedReviews, nextPageToken);
    }
}
