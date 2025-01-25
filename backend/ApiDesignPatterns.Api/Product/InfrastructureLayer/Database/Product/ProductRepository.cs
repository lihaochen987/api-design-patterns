using System.Data;
using backend.Product.DomainModels;
using backend.Product.DomainModels.ValueObjects;
using Dapper;

namespace backend.Product.InfrastructureLayer.Database.Product;

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

    public async Task<PetFood?> GetPetFoodProductAsync(long id)
    {
        var product = await dbConnection.QueryAsync<PetFood, Dimensions, Pricing, PetFood>(
            ProductQueries.GetPetFoodProduct,
            (petFood, dimensions, pricing) =>
            {
                petFood.Dimensions = dimensions;
                petFood.Pricing = pricing;
                return petFood;
            },
            new { Id = id },
            splitOn: "Length,BasePrice");

        return product.SingleOrDefault();
    }

    public async Task<GroomingAndHygiene?> GetGroomingAndHygieneProductAsync(long id)
    {
        var product = await dbConnection.QueryAsync<GroomingAndHygiene, Dimensions, Pricing, GroomingAndHygiene>(
            ProductQueries.GetGroomingAndHygieneProduct,
            (petFood, dimensions, pricing) =>
            {
                petFood.Dimensions = dimensions;
                petFood.Pricing = pricing;
                return petFood;
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
        await dbConnection.ExecuteAsync(
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

    public async Task<long> UpdateProductAsync(DomainModels.Product product)
    {
        return await dbConnection.ExecuteScalarAsync<long>(
            ProductQueries.UpdateProduct,
            new
            {
                product.Id,
                product.Name,
                product.Category,
                product.Dimensions.Width,
                product.Dimensions.Height,
                product.Dimensions.Length,
                product.Pricing.BasePrice,
                product.Pricing.DiscountPercentage,
                product.Pricing.TaxRate,
            }
        );
    }

    public async Task UpdatePetFoodProductAsync(PetFood product)
    {
        await dbConnection.ExecuteAsync(
            ProductQueries.UpdatePetFoodProduct,
            new
            {
                product.Id,
                product.AgeGroup,
                product.BreedSize,
                product.Ingredients,
                product.StorageInstructions,
                product.WeightKg
            }
        );
    }

    public async Task UpdateGroomingAndHygieneProductAsync(GroomingAndHygiene product)
    {
        await dbConnection.ExecuteAsync(
            ProductQueries.UpdateGroomingAndHygieneProduct,
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
}
