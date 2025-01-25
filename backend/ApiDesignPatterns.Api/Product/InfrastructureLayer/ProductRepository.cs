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

    public async Task<long> CreateProductAsync(DomainModels.Product product)
    {
        return await dbConnection.ExecuteScalarAsync<long>(
            ProductQueries.CreateProduct,
            new
            {
                product.Name,
                product.Dimensions.Length,
                product.Dimensions.Width,
                product.Dimensions.Height,
                product.Category,
                product.Pricing.BasePrice,
                product.Pricing.DiscountPercentage,
                product.Pricing.TaxRate
            }
        );
    }

    public async Task CreatePetFoodProductAsync(PetFood product)
    {
        await dbConnection.ExecuteAsync(
            ProductQueries.CreatePetFoodProduct,
            new
            {
                product.Id,
                product.AgeGroup,
                product.BreedSize,
                product.Ingredients,
                product.NutritionalInfo,
                product.StorageInstructions,
                product.WeightKg
            }
        );
    }

    public async Task CreateGroomingAndHygieneProductAsync(GroomingAndHygiene product)
    {
        await dbConnection.ExecuteScalarAsync(
            ProductQueries.CreateGroomingAndHygieneProduct,
            new
            {
                product.Id,
                product.IsNatural,
                product.IsHypoallergenic,
                product.UsageInstructions,
                product.IsCrueltyFree,
                product.SafetyWarnings
            }
        );
    }

    public async Task DeleteProductAsync(long id)
    {
        await dbConnection.ExecuteAsync(ProductQueries.DeleteProduct, new { Id = id });
    }

    public async Task UpdateProductAsync(DomainModels.Product product)
    {
        dbConnection.Open();
        using var transaction = dbConnection.BeginTransaction();
        try
        {
            product.Id = await dataWriter.UpdateProduct(product, transaction);
            switch (product)
            {
                case PetFood petFood:
                    await dataWriter.UpdatePetFoodProduct(petFood, transaction);
                    break;
                case GroomingAndHygiene groomingAndHygiene:
                    await dataWriter.UpdateGroomingAndHygieneProduct(groomingAndHygiene, transaction);
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
}
