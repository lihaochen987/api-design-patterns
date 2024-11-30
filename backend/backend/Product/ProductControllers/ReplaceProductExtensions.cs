using System.Globalization;
using backend.Product.Contracts;
using backend.Product.DomainModels;
using backend.Shared;

namespace backend.Product.ProductControllers;

public class ReplaceProductExtensions(TypeParser typeParser)
{
    public BaseProduct ToEntity(ReplaceProductRequest request)
    {
        // ProductPricing fields
        if (!decimal.TryParse(request.Pricing.DiscountPercentage, out var discountPercentage))
            throw new ArgumentException("Invalid discount percentage");
        if (!decimal.TryParse(request.Pricing.TaxRate, out var taxRate))
            throw new ArgumentException("Invalid tax rate");
        var basePrice = typeParser.ParseDecimal(request.Pricing.BasePrice, "Invalid BasePrice");

        // Product fields
        var category = typeParser.ParseEnum<Category>(request.Category, "Invalid product category");

        // Dimensions Fields
        var length = typeParser.ParseDecimal(request.Dimensions.Length, "Invalid dimensions length");
        var width = typeParser.ParseDecimal(request.Dimensions.Width, "Invalid dimensions width");
        var height = typeParser.ParseDecimal(request.Dimensions.Height, "Invalid dimensions height");

        var dimensions = new Dimensions(length, width, height);
        var pricing = new Pricing(basePrice, discountPercentage, taxRate);
        return new BaseProduct(request.Name, pricing, category, dimensions);
    }

    public ReplaceProductResponse ToReplaceProductResponse(DomainModels.Product product)
    {
        var response = new ReplaceProductResponse
        {
            Name = product.Name,
            Category = product.Category.ToString(),
            Pricing = new ProductPricingContract
            {
                BasePrice = product.Pricing.BasePrice.ToString(CultureInfo.InvariantCulture),
                DiscountPercentage = product.Pricing.DiscountPercentage.ToString(CultureInfo.InvariantCulture),
                TaxRate = product.Pricing.TaxRate.ToString(CultureInfo.InvariantCulture)
            },
            Dimensions = new DimensionsContract
            {
                Length = product.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Width = product.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Height = product.Dimensions.Height.ToString(CultureInfo.InvariantCulture)
            }
        };

        if (product is PetFood petFood)
        {
            response.AgeGroup = petFood.AgeGroup.ToString();
            response.BreedSize = petFood.BreedSize.ToString();
            response.Ingredients = petFood.Ingredients;
            response.NutritionalInfo =
                typeParser.ParseDictionaryToString(petFood.NutritionalInfo, "Invalid nutritional info.");
            response.StorageInstructions = petFood.StorageInstructions;
            response.WeightKg = petFood.WeightKg.ToString(CultureInfo.InvariantCulture);
        }

        return response;
    }

    public ReplaceProductRequest ToReplaceProductRequest(DomainModels.Product product)
    {
        return new ReplaceProductRequest
        {
            Name = product.Name,
            Category = product.Category.ToString(),
            Pricing = new ProductPricingContract
            {
                BasePrice = product.Pricing.BasePrice.ToString(CultureInfo.InvariantCulture),
                DiscountPercentage = product.Pricing.DiscountPercentage.ToString(CultureInfo.InvariantCulture),
                TaxRate = product.Pricing.TaxRate.ToString(CultureInfo.InvariantCulture)
            },
            Dimensions = new DimensionsContract
            {
                Length = product.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Width = product.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Height = product.Dimensions.Height.ToString(CultureInfo.InvariantCulture)
            }
        };
    }
}