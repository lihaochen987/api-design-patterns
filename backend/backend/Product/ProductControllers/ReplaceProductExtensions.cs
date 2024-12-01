using System.Globalization;
using backend.Product.Contracts;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Shared;

namespace backend.Product.ProductControllers;

public class ReplaceProductExtensions(TypeParser typeParser)
{
    public DomainModels.Product ToEntity(ReplaceProductRequest request)
    {
        // ProductPricing fields
        if (!decimal.TryParse(request.Pricing.DiscountPercentage, out decimal discountPercentage))
        {
            throw new ArgumentException("Invalid discount percentage");
        }

        if (!decimal.TryParse(request.Pricing.TaxRate, out decimal taxRate))
        {
            throw new ArgumentException("Invalid tax rate");
        }

        decimal basePrice = typeParser.ParseDecimal(request.Pricing.BasePrice, "Invalid BasePrice");

        // Product fields
        Category category = typeParser.ParseEnum<Category>(request.Category, "Invalid product category");

        // Dimensions Fields
        decimal length = typeParser.ParseDecimal(request.Dimensions.Length, "Invalid dimensions length");
        decimal width = typeParser.ParseDecimal(request.Dimensions.Width, "Invalid dimensions width");
        decimal height = typeParser.ParseDecimal(request.Dimensions.Height, "Invalid dimensions height");

        Dimensions dimensions = new(length, width, height);
        Pricing pricing = new(basePrice, discountPercentage, taxRate);

        // PetFood
        if (category == Category.PetFood)
        {
            AgeGroup ageGroup = typeParser.ParseEnum<AgeGroup>(request.AgeGroup, "Invalid age group");
            BreedSize breedSize = typeParser.ParseEnum<BreedSize>(request.BreedSize, "Invalid breed size");
            decimal weight = typeParser.ParseDecimal(request.WeightKg, "Invalid weight");
            string? ingredients = string.IsNullOrWhiteSpace(request.Ingredients)
                ? throw new ArgumentException("Ingredients cannot be null or whitespace.")
                : request.Ingredients;
            if (request.NutritionalInfo == null)
            {
                throw new ArgumentException("Nutritional info cannot be null.");
            }

            string? storageInstructions = string.IsNullOrWhiteSpace(request.StorageInstructions)
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
            bool isNatural = typeParser.ParseBool(request.IsNatural, "Invalid IsNatural boolean");
            bool isHypoAllergenic = typeParser.ParseBool(request.IsHypoAllergenic, "Invalid IsHypoAllergenic boolean");
            bool isCrueltyFree = typeParser.ParseBool(request.IsCrueltyFree, "Invalid IsCrueltyFree boolean");
            string usageInstructions = typeParser.ParseString(request.UsageInstructions, "Invalid Usage Instructions");
            string safetyWarnings = typeParser.ParseString(request.SafetyWarnings, "Invalid Safety Warnings");

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
}
