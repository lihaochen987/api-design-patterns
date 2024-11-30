using System.Globalization;
using backend.Product.Contracts;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Shared;
using ArgumentException = System.ArgumentException;

namespace backend.Product.ProductControllers;

public class CreateProductExtensions(TypeParser typeParser)
{
    public DomainModels.Product ToEntity(CreateProductRequest request)
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

        // PetFood
        if (category == Category.PetFood)
        {
            var ageGroup = typeParser.ParseEnum<AgeGroup>(request.AgeGroup, "Invalid age group");
            var breedSize = typeParser.ParseEnum<BreedSize>(request.BreedSize, "Invalid breed size");
            var weight = typeParser.ParseDecimal(request.WeightKg, "Invalid weight");
            var ingredients = string.IsNullOrWhiteSpace(request.Ingredients)
                ? throw new ArgumentException("Ingredients cannot be null or whitespace.")
                : request.Ingredients;
            if (request.NutritionalInfo == null)
            {
                throw new ArgumentException("Nutritional info cannot be null.");
            }

            var storageInstructions = string.IsNullOrWhiteSpace(request.StorageInstructions)
                ? throw new ArgumentException("Storage instructions cannot be null or whitespace.")
                : request.StorageInstructions;

            return new PetFood(
                request.Name,
                pricing,
                dimensions,
                ageGroup,
                breedSize,
                ingredients,
                request.NutritionalInfo,
                storageInstructions,
                weight);
        }

        if (category == Category.GroomingAndHygiene)
        {
            var isNatural = typeParser.ParseBool(request.IsNatural, "Invalid IsNatural boolean");
            var isHypoAllergenic = typeParser.ParseBool(request.IsHypoAllergenic, "Invalid IsHypoAllergenic boolean");
            var isCrueltyFree = typeParser.ParseBool(request.IsCrueltyFree, "Invalid IsCrueltyFree boolean");
            var usageInstructions = typeParser.ParseString(request.UsageInstructions, "Invalid Usage Instructions");
            var safetyWarnings = typeParser.ParseString(request.SafetyWarnings, "Invalid Safety Warnings");

            return new GroomingAndHygiene(
                request.Name,
                pricing,
                dimensions,
                isNatural,
                isHypoAllergenic,
                usageInstructions,
                isCrueltyFree,
                safetyWarnings);
        }

        return new BaseProduct(request.Name, pricing, category, dimensions);
    }

    public CreateProductResponse ToCreateProductResponse(DomainModels.Product product)
    {
        var response = new CreateProductResponse
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
            },
        };

        if (product is PetFood petFood)
        {
            response.AgeGroup = petFood.AgeGroup.ToString();
            response.BreedSize = petFood.BreedSize.ToString();
            response.Ingredients = petFood.Ingredients;
            response.NutritionalInfo =
                typeParser.ParseDictionaryToString(petFood.NutritionalInfo, "Invalid nutritional info");
            response.StorageInstructions = petFood.StorageInstructions;
            response.WeightKg = petFood.WeightKg.ToString(CultureInfo.InvariantCulture);
        }

        if (product is GroomingAndHygiene groomingAndHygiene)
        {
            response.IsNatural = groomingAndHygiene.IsNatural;
            response.IsHypoAllergenic = groomingAndHygiene.IsHypoallergenic;
            response.UsageInstructions = groomingAndHygiene.UsageInstructions;
            response.IsCrueltyFree = groomingAndHygiene.IsCrueltyFree;
            response.SafetyWarnings = groomingAndHygiene.SafetyWarnings;
        }

        return response;
    }

    public CreateProductRequest ToCreateProductRequest(DomainModels.Product product)
    {
        var request = new CreateProductRequest
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
            request.AgeGroup = petFood.AgeGroup.ToString();
            request.BreedSize = petFood.BreedSize.ToString();
            request.Ingredients = petFood.Ingredients;
            request.NutritionalInfo = petFood.NutritionalInfo;
            request.StorageInstructions = petFood.StorageInstructions;
            request.WeightKg = petFood.WeightKg.ToString(CultureInfo.InvariantCulture);
        }
        if (product is GroomingAndHygiene groomingAndHygiene)
        {
            request.IsNatural = groomingAndHygiene.IsNatural;
            request.IsHypoAllergenic = groomingAndHygiene.IsHypoallergenic;
            request.UsageInstructions = groomingAndHygiene.UsageInstructions;
            request.IsCrueltyFree = groomingAndHygiene.IsCrueltyFree;
            request.SafetyWarnings = groomingAndHygiene.SafetyWarnings;
        }

        return request;
    }
}