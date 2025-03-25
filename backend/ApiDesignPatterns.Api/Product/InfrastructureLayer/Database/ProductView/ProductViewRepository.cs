using System.Data;
using System.Text;
using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.DomainModels.ValueObjects;
using backend.Shared;
using Dapper;
using SqlFilterBuilder = backend.Shared.SqlFilterBuilder;

namespace backend.Product.InfrastructureLayer.Database.ProductView;

public class ProductViewRepository(
    IDbConnection dbConnection,
    SqlFilterBuilder productSqlFilterBuilder)
    : IProductViewRepository
{
    public async Task<DomainModels.Views.ProductView?> GetProductView(long id)
    {
        var product = await dbConnection
            .QueryAsync<DomainModels.Views.ProductView, Dimensions, DomainModels.Views.ProductView>(
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

    // Todo: Refactor this
    public async Task<PagedProducts> ListProductsAsync(
        string? pageToken,
        string? filter,
        int maxPageSize)
    {
        var parameters = new DynamicParameters();
        var filterClause = new StringBuilder();

        // Add Filter SQL query
        if (!string.IsNullOrEmpty(filter))
        {
            string sqlFilterClause = productSqlFilterBuilder.BuildSqlWhereClause(filter);
            filterClause.Append($" AND {sqlFilterClause}");
        }

        // Add Page Token SQL query
        string seekClause = string.Empty;
        if (!string.IsNullOrEmpty(pageToken))
        {
            if (long.TryParse(pageToken, out long lastSeenId))
            {
                seekClause = " AND product_id > @LastSeenId";
                parameters.Add("LastSeenId", lastSeenId);
            }
        }

        // Get count based on filter
        string countSql = $"SELECT COUNT(*) FROM products_view WHERE 1=1{filterClause}";
        int totalCount = await dbConnection.ExecuteScalarAsync<int>(countSql, parameters);

        // Build the data query with both filter and seek pagination
        var dataSql = new StringBuilder(ProductViewQueries.ListProductsBase);
        dataSql.Append(filterClause);
        dataSql.Append(seekClause);
        dataSql.Append(" ORDER BY product_id");
        dataSql.Append(" LIMIT @PageSize");
        parameters.Add("PageSize", maxPageSize + 1);

        var products = (await dbConnection
            .QueryAsync<DomainModels.Views.ProductView, Dimensions, DomainModels.Views.ProductView>(
                dataSql.ToString(),
                (product, dimensions) =>
                {
                    product.Dimensions = dimensions;
                    return product;
                },
                parameters,
                splitOn: "Length"
            )).ToList();

        string? nextPageToken = null;
        var resultProducts = products.Take(maxPageSize).ToList();

        if (products.Count > maxPageSize)
        {
            nextPageToken = resultProducts.Last().Id.ToString();
        }

        return new PagedProducts(resultProducts, nextPageToken, totalCount);
    }
}
