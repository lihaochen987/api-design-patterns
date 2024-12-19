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
        decimal discountPercentage =
            typeParser.ParseDecimal(request.Pricing.DiscountPercentage, "Invalid discount percentage");
        decimal taxRate = typeParser.ParseDecimal(request.Pricing.TaxRate, "Invalid tax rate");
        decimal basePrice = typeParser.ParseDecimal(request.Pricing.BasePrice, "Invalid BasePrice");

        // Product fields
        Category category = typeParser.ParseEnum<Category>(request.Category, "Invalid product category");

        // Dimensions Fields
        decimal length = typeParser.ParseDecimal(request.Dimensions.Length, "Invalid dimensions length");
        decimal width = typeParser.ParseDecimal(request.Dimensions.Width, "Invalid dimensions width");
        decimal height = typeParser.ParseDecimal(request.Dimensions.Height, "Invalid dimensions height");

        Dimensions dimensions = new() { Length = length, Width = width, Height = height };
        Pricing pricing = new() { BasePrice = basePrice, DiscountPercentage = discountPercentage, TaxRate = taxRate };

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

            return new PetFood
            {
                Name = request.Name,
                Category = Category.PetFood,
                Pricing = pricing,
                Dimensions = dimensions,
                AgeGroup = ageGroup,
                BreedSize = breedSize,
                Ingredients = ingredients,
                NutritionalInfo = request.NutritionalInfo,
                StorageInstructions = storageInstructions,
                WeightKg = weight
            };
        }

        if (category == Category.GroomingAndHygiene)
        {
            bool isNatural = typeParser.ParseBool(request.IsNatural, "Invalid IsNatural boolean");
            bool isHypoAllergenic = typeParser.ParseBool(request.IsHypoAllergenic, "Invalid IsHypoAllergenic boolean");
            bool isCrueltyFree = typeParser.ParseBool(request.IsCrueltyFree, "Invalid IsCrueltyFree boolean");
            string usageInstructions = typeParser.ParseString(request.UsageInstructions, "Invalid Usage Instructions");
            string safetyWarnings = typeParser.ParseString(request.SafetyWarnings, "Invalid Safety Warnings");

            return new GroomingAndHygiene
            {
                Name = request.Name,
                Category = Category.GroomingAndHygiene,
                Pricing = pricing,
                Dimensions = dimensions,
                IsNatural = isNatural,
                IsHypoallergenic = isHypoAllergenic,
                UsageInstructions = usageInstructions,
                IsCrueltyFree = isCrueltyFree,
                SafetyWarnings = safetyWarnings
            };
        }

        return new DomainModels.Product
        {
            Name = request.Name, Category = category, Pricing = pricing, Dimensions = dimensions
        };
    }
}
