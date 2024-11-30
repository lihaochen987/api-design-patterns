using backend.Product.DomainModels;
using backend.Product.ProductControllers;

namespace backend.Product.FieldMasks;

/// <summary>
/// ProductMaskFieldPaths is a class which holds all the manual changes required for this API to work.
/// For instance when adding a new field or value object you would have to:
/// 1. Add the new object to the AllFieldPaths (partial retrievals)
/// 2. Add parsing logic for GetUpdatedProductValues (partial updates)
/// 3. Add the mapping in the extension methods (TBC on making this more generic and easier)
/// </summary>
public class ProductFieldMaskConfiguration
{
    public readonly HashSet<string> ProductFieldPaths =
    [
        "*",
        "id",
        "name",
        "category",
        "price",
        "dimensions.*",
        "dimensions.width",
        "dimensions.height",
        "dimensions.length",
        "agegroup",
        "breedsize",
        "nutritionalinfo",
        "ingredients",
        "weightkg",
        "storageinstructions"
    ];

    public (
        string name,
        Pricing pricing,
        Category category,
        Dimensions dimensions)
        GetUpdatedProductValues(
            UpdateProductRequest request,
            DomainModels.Product baseProduct)
    {
        var name = request.FieldMask.Contains("name", StringComparer.OrdinalIgnoreCase)
                   && !string.IsNullOrEmpty(request.Name)
            ? request.Name
            : baseProduct.Name;

        var category = request.FieldMask.Contains("category", StringComparer.OrdinalIgnoreCase)
                       && Enum.TryParse(request.Category, true, out Category parsedCategory)
            ? parsedCategory
            : baseProduct.Category;

        var dimensions = GetUpdatedDimensionValues(request, baseProduct.Dimensions);
        var pricing = GetUpdatedProductPricingValues(request, baseProduct.Pricing);

        return (name, pricing, category, dimensions);
    }

    private static Dimensions GetUpdatedDimensionValues(
        UpdateProductRequest request,
        Dimensions currentDimensions)
    {
        var length = request.FieldMask.Contains("dimensions.length", StringComparer.OrdinalIgnoreCase)
                     && !string.IsNullOrEmpty(request.Dimensions.Length)
            ? decimal.Parse(request.Dimensions.Length)
            : currentDimensions.Length;

        var width = request.FieldMask.Contains("dimensions.width", StringComparer.OrdinalIgnoreCase)
                    && !string.IsNullOrEmpty(request.Dimensions.Width)
            ? decimal.Parse(request.Dimensions.Width)
            : currentDimensions.Width;

        var height = request.FieldMask.Contains("dimensions.height", StringComparer.OrdinalIgnoreCase)
                     && !string.IsNullOrEmpty(request.Dimensions.Height)
            ? decimal.Parse(request.Dimensions.Height)
            : currentDimensions.Height;

        return new Dimensions(length, width, height);
    }

    public (
        AgeGroup ageGroup,
        BreedSize breedSize,
        string ingredients,
        Dictionary<string, object> nutritionalInfo,
        string storageInstructions,
        decimal weightKg)
        GetUpdatedPetFoodValues(
            UpdateProductRequest request,
            PetFood petFood)
    {
        var ageGroup = request.FieldMask.Contains("agegroup", StringComparer.OrdinalIgnoreCase)
                       && Enum.TryParse(request.Category, true, out AgeGroup parsedAgeGroup)
            ? parsedAgeGroup
            : petFood.AgeGroup;

        var breedSize = request.FieldMask.Contains("breedsize", StringComparer.OrdinalIgnoreCase)
                        && Enum.TryParse(request.Category, true, out BreedSize parsedBreedSize)
            ? parsedBreedSize
            : petFood.BreedSize;

        var ingredients = request.FieldMask.Contains("ingredients", StringComparer.OrdinalIgnoreCase)
                          && !string.IsNullOrEmpty(request.Ingredients)
            ? request.Ingredients
            : petFood.Ingredients;

        var nutritionalInfo = request.FieldMask.Contains("nutritionalinfo", StringComparer.OrdinalIgnoreCase)
                              && request.NutritionalInfo is { } parsedNutritionalInfo
            ? parsedNutritionalInfo
            : petFood.NutritionalInfo;

        var storageInstructions = request.FieldMask.Contains("storageinstructions", StringComparer.OrdinalIgnoreCase)
                                  && !string.IsNullOrEmpty(request.StorageInstructions)
            ? request.StorageInstructions
            : petFood.StorageInstructions;

        var weight = request.FieldMask.Contains("weightkg", StringComparer.OrdinalIgnoreCase)
                     && !string.IsNullOrEmpty(request.WeightKg)
            ? decimal.Parse(request.WeightKg)
            : petFood.WeightKg;

        return (ageGroup, breedSize, ingredients, nutritionalInfo, storageInstructions, weight);
    }

    private static Pricing
        GetUpdatedProductPricingValues(
            UpdateProductRequest request,
            Pricing product)
    {
        var basePrice = request.FieldMask.Contains("baseprice", StringComparer.OrdinalIgnoreCase)
                        && decimal.TryParse(request.Pricing.BasePrice, out var parsedBasePrice)
            ? parsedBasePrice
            : product.BasePrice;

        var discountPercentage = request.FieldMask.Contains("discountpercentage", StringComparer.OrdinalIgnoreCase)
                                 && decimal.TryParse(request.Pricing.DiscountPercentage,
                                     out var parsedDiscountPercentage)
            ? parsedDiscountPercentage!
            : product.DiscountPercentage;

        var taxRate = request.FieldMask.Contains("taxrate", StringComparer.OrdinalIgnoreCase)
                      && decimal.TryParse(request.Pricing.TaxRate, out var parsedTaxRate)
            ? parsedTaxRate!
            : product.TaxRate;

        return new Pricing(basePrice, discountPercentage, taxRate);
    }
}