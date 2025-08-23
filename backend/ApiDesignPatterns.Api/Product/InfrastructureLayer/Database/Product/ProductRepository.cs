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
            Category.PetFood => await GetPetFoodProductAsync(id),
            Category.GroomingAndHygiene => await GetGroomingAndHygieneProductAsync(id),
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

    public async Task<PetFood?> GetPetFoodProductAsync(long id)
    {
        var product = await dbConnection.QueryAsync<PetFood, Dimensions, Pricing, PetFood>(
            ProductQueries.GetPetFoodProduct,
            (petFood, dimensions, pricing) =>
            {
                var petFoodResult = petFood with { Pricing = pricing, Dimensions = dimensions };
                return petFoodResult;
            },
            new { Id = id },
            splitOn: "Length,BasePrice");

        return product.SingleOrDefault();
    }

    public async Task<GroomingAndHygiene?> GetGroomingAndHygieneProductAsync(long id)
    {
        var product = await dbConnection.QueryAsync<GroomingAndHygiene, Dimensions, Pricing, GroomingAndHygiene>(
            ProductQueries.GetGroomingAndHygieneProduct,
            (groomingAndHygiene, dimensions, pricing) =>
            {
                var groomingAndHygieneResult = groomingAndHygiene with { Pricing = pricing, Dimensions = dimensions };
                return groomingAndHygieneResult;
            },
            new { Id = id },
            splitOn: "Length,BasePrice");

        return product.SingleOrDefault();
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
                await CreatePetFoodProductAsync(petFood);
                break;
            case GroomingAndHygiene groomingProduct:
                await CreateGroomingAndHygieneProductAsync(groomingProduct);
                break;
        }

        return productId;
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
                Name = product.Name.Value,
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
                await UpdatePetFoodProductAsync(petFood);
                break;
            case GroomingAndHygiene groomingProduct:
                await UpdateGroomingAndHygieneProductAsync(groomingProduct);
                break;
        }

        return result;
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
                Ingredients = product.Ingredients.Value,
                StorageInstructions = product.StorageInstructions.Value,
                WeightKg = product.WeightKg.Value
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
                UsageInstructions = product.UsageInstructions.Value,
                product.IsCrueltyFree,
                SafetyWarnings = product.SafetyWarnings.Value
            }
        );
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
            Ingredients = product.Ingredients.Value,
            StorageInstructions = product.StorageInstructions.Value,
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
            SafetyWarnings = product.SafetyWarnings.Value
        }).ToList();

        await dbConnection.ExecuteAsync(
            ProductQueries.CreateGroomingAndHygieneProduct,
            parametersList
        );
    }
}
