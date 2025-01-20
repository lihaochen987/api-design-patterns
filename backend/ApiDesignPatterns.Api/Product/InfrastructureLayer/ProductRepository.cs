using System.Data;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.InfrastructureLayer.Queries;
using Dapper;

namespace backend.Product.InfrastructureLayer;

public class ProductRepository(IDbConnection dbConnection) : IProductRepository
{
    public async Task<DomainModels.Product?> GetProductAsync(long id)
    {
        var product = await dbConnection.QueryAsync<DomainModels.Product, Dimensions, Pricing, DomainModels.Product>(
            ProductQueries.GetProduct,
            (product, dimensions, pricing) =>
            {
                product.Dimensions = dimensions;
                product.Pricing = pricing;
                return product;
            },
            new { Id = id },
            splitOn: "Length,BasePrice");

        return product.SingleOrDefault();
    }

    public async Task CreateProductAsync(DomainModels.Product product)
    {
        await dbConnection.ExecuteAsync(ProductQueries.CreateProduct,
            new { product.Id, });
    }

    public async Task DeleteProductAsync(long id)
    {
        await dbConnection.ExecuteAsync(ProductQueries.DeleteProduct, new { Id = id });
    }

    public async Task UpdateProductAsync(DomainModels.Product product)
    {
        await dbConnection.ExecuteAsync(ProductQueries.UpdateProduct,
            new { product.Id, });
    }
}
