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
    PaginateService<DomainModels.Views.ProductView> paginateService,
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

    public async Task<PagedProducts> ListProductsAsync(
        string? pageToken,
        string? filter,
        int maxPageSize)
    {
        var parameters = new DynamicParameters();
        var whereClause = new StringBuilder();

        // Pagination filter
        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenProduct))
        {
            whereClause.Append(" AND product_id > @LastSeenProduct");
            parameters.Add("LastSeenProduct", lastSeenProduct);
        }

        // Custom filter
        if (!string.IsNullOrEmpty(filter))
        {
            string filterClause = productSqlFilterBuilder.BuildSqlWhereClause(filter);
            whereClause.Append($" AND {filterClause}");
        }

        string countSql = $"SELECT COUNT(*) FROM products_view WHERE 1=1{whereClause}";
        int totalCount = await dbConnection.ExecuteScalarAsync<int>(countSql, parameters);

        var dataSql = new StringBuilder(ProductViewQueries.ListProductsBase);
        dataSql.Append(whereClause);

        dataSql.Append(" ORDER BY product_id LIMIT @PageSizePlusOne");
        parameters.Add("PageSizePlusOne", maxPageSize + 1);

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

        List<DomainModels.Views.ProductView> paginatedProducts =
            paginateService.Paginate(products, maxPageSize, out string? nextPageToken);

        return new PagedProducts(paginatedProducts, nextPageToken, totalCount);
    }
}
