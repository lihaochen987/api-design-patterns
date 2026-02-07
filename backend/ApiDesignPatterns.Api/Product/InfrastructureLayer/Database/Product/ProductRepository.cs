using System.Data;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using Dapper;

namespace backend.Product.InfrastructureLayer.Database.Product;

public class ProductRepository(IDbConnection dbConnection)
    : IProductRepository, IGetProduct, ICreateProduct, IDeleteProduct, IUpdateProduct
{
    public async Task<DomainModels.Product?> GetProductAsync(long id)
    {
        var basicProduct =
            await dbConnection.QueryAsync<DomainModels.Product, Dimensions, Pricing, DomainModels.Product>(
                ProductQueries.GetProduct,
                (product, dimensions, pricing) => new DomainModels.Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = product.Category,
                    Pricing = pricing,
                    Dimensions = dimensions
                },
                new { Id = id },
                splitOn: "Length,BasePrice");

        var baseProduct = basicProduct.SingleOrDefault();
        if (baseProduct == null) return null;

        return baseProduct.Category switch
        {
            Category.PetFood => (await dbConnection.QueryAsync<PetFood, Dimensions, Pricing, PetFood>(
                ProductQueries.GetPetFoodProduct,
                (petFood, dimensions, pricing) => petFood with { Pricing = pricing, Dimensions = dimensions },
                new { Id = id },
                splitOn: "Length,BasePrice")).SingleOrDefault(),

            Category.GroomingAndHygiene => (await dbConnection.QueryAsync<GroomingAndHygiene, Dimensions, Pricing, GroomingAndHygiene>(
                ProductQueries.GetGroomingAndHygieneProduct,
                (groomingAndHygiene, dimensions, pricing) => groomingAndHygiene with { Pricing = pricing, Dimensions = dimensions },
                new { Id = id },
                splitOn: "Length,BasePrice")).SingleOrDefault(),

            _ => baseProduct
        };
    }

    public async Task<List<DomainModels.Product>> GetProductsByIds(List<long> productIds)
    {
        if (productIds.Count == 0)
        {
            return [];
        }

        var products = await dbConnection.QueryAsync<DomainModels.Product, Dimensions, Pricing, DomainModels.Product>(
            ProductQueries.GetProductsByIds,
            (product, dimensions, pricing) =>
            {
                var productResult = new DomainModels.Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = product.Category,
                    Pricing = pricing,
                    Dimensions = dimensions
                };
                return productResult;
            },
            new { ProductIds = productIds },
            splitOn: "Length,BasePrice");

        return products.ToList();
    }

    public async Task<long> CreateProductAsync(DomainModels.Product product)
    {
        long productId = await dbConnection.ExecuteScalarAsync<long>(
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

        var productWithId = product with { Id = productId };

        switch (productWithId)
        {
            case PetFood petFood:
                await dbConnection.ExecuteAsync(
                    ProductQueries.CreatePetFoodProduct,
                    new
                    {
                        petFood.Id,
                        petFood.AgeGroup,
                        petFood.BreedSize,
                        petFood.Ingredients,
                        petFood.StorageInstructions,
                        petFood.WeightKg
                    }
                );
                break;
            case GroomingAndHygiene groomingProduct:
                await dbConnection.ExecuteAsync(
                    ProductQueries.CreateGroomingAndHygieneProduct,
                    new
                    {
                        groomingProduct.Id,
                        groomingProduct.IsNatural,
                        groomingProduct.IsHypoallergenic,
                        groomingProduct.UsageInstructions,
                        groomingProduct.IsCrueltyFree,
                        groomingProduct.SafetyWarnings
                    }
                );
                break;
        }

        return productId;
    }

    public async Task DeleteProductAsync(long id)
    {
        await dbConnection.ExecuteAsync(ProductQueries.DeleteProduct, new { Id = id });
    }

    public async Task DeleteProductsAsync(IEnumerable<long> ids)
    {
        await dbConnection.ExecuteAsync(ProductQueries.DeleteProducts, new { Ids = ids });
    }

    public async Task<long> UpdateProductAsync(DomainModels.Product product)
    {
        long result = await dbConnection.ExecuteScalarAsync<long>(
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

        switch (product)
        {
            case PetFood petFood:
                await dbConnection.ExecuteAsync(
                    ProductQueries.UpdatePetFoodProduct,
                    new
                    {
                        petFood.Id,
                        petFood.AgeGroup,
                        petFood.BreedSize,
                        petFood.Ingredients,
                        petFood.StorageInstructions,
                        WeightKg = petFood.WeightKg.Value
                    }
                );
                break;
            case GroomingAndHygiene groomingProduct:
                await dbConnection.ExecuteAsync(
                    ProductQueries.UpdateGroomingAndHygieneProduct,
                    new
                    {
                        groomingProduct.Id,
                        groomingProduct.IsNatural,
                        groomingProduct.IsHypoallergenic,
                        UsageInstructions = groomingProduct.UsageInstructions.Value,
                        groomingProduct.IsCrueltyFree,
                        groomingProduct.SafetyWarnings
                    }
                );
                break;
        }

        return result;
    }

    public async Task<IEnumerable<long>> CreateProductsAsync(IEnumerable<DomainModels.Product> products)
    {
        var results = new List<long>();

        foreach (var product in products)
        {
            long id = await dbConnection.ExecuteScalarAsync<long>(
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
            results.Add(id);
        }

        return results;
    }

    public async Task CreatePetFoodProductsAsync(IEnumerable<PetFood> petFoodProducts)
    {
        var parametersList = petFoodProducts.Select(product => new
        {
            product.Id,
            product.AgeGroup,
            product.BreedSize,
            product.Ingredients,
            product.StorageInstructions,
            WeightKg = product.WeightKg.Value
        }).ToList();

        await dbConnection.ExecuteAsync(
            ProductQueries.CreatePetFoodProduct,
            parametersList
        );
    }

    public async Task CreateGroomingAndHygieneProductsAsync(IEnumerable<GroomingAndHygiene> groomingProducts)
    {
        var parametersList = groomingProducts.Select(product => new
        {
            product.Id,
            product.IsNatural,
            product.IsHypoallergenic,
            UsageInstructions = product.UsageInstructions.Value,
            product.IsCrueltyFree,
             product.SafetyWarnings
        }).ToList();

        await dbConnection.ExecuteAsync(
            ProductQueries.CreateGroomingAndHygieneProduct,
            parametersList
        );
    }
}
