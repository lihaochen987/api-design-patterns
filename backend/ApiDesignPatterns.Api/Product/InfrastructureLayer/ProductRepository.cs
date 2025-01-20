using System.Data;
using backend.Product.DomainModels;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.InfrastructureLayer.Queries;
using backend.Product.Services;
using Dapper;

namespace backend.Product.InfrastructureLayer;

public class ProductRepository(IDbConnection dbConnection, ProductDataWriter dataWriter) : IProductRepository
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
        dbConnection.Open();
        using var transaction = dbConnection.BeginTransaction();

        try
        {
            long id = await dataWriter.CreateProduct(product, transaction);
            switch (product)
            {
                case PetFood petFood:
                    petFood.Id = id;
                    await dataWriter.CreatePetFoodProduct(petFood, transaction);
                    break;
                case GroomingAndHygiene groomingAndHygiene:
                    groomingAndHygiene.Id = id;
                    await dataWriter.CreateGroomingAndHygieneProduct(groomingAndHygiene, transaction);
                    break;
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            dbConnection.Close();
        }
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
