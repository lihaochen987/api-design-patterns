using System.Data;
using System.Text;
using backend.Product.DomainModels.ValueObjects;
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
        var product = await dbConnection.QueryAsync<ProductView, Dimensions, ProductView>(
            ProductViewQueries.GetProductView,
            (product, dimensions) =>
            {
                product.Dimensions = dimensions;
                return product;
            },
            new { Id = id },
            splitOn: "Length"
        );

        return product.SingleOrDefault();
    }

    public async Task<(List<ProductView>, string?)> ListProductsAsync(
        string? pageToken,
        string? filter,
        int maxPageSize)
    {
        var sql = new StringBuilder(ProductViewQueries.ListProductsBase);
        var parameters = new DynamicParameters();

        // Pagination filter
        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenProduct))
        {
            sql.Append(" AND product_id > @LastSeenProduct");
            parameters.Add("LastSeenProduct", lastSeenProduct);
        }

        // Custom filter
        if (!string.IsNullOrEmpty(filter))
        {
            string filterClause = productSqlFilterBuilder.BuildSqlWhereClause(filter);
            sql.Append($" AND {filterClause}");
        }

        // Order and limit
        sql.Append(" ORDER BY product_id LIMIT @PageSizePlusOne");
        parameters.Add("PageSizePlusOne", maxPageSize + 1);

        var products = (await dbConnection.QueryAsync<ProductView, Dimensions, ProductView>(
            sql.ToString(),
            (product, dimensions) =>
            {
                product.Dimensions = dimensions;
                return product;
            },
            parameters,
            splitOn: "Length"
        )).ToList();
        List<ProductView> paginatedProducts = queryService.Paginate(products, maxPageSize, out string? nextPageToken);

        return (paginatedProducts, nextPageToken);
    }
}
