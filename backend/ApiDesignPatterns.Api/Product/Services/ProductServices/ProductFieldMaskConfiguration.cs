using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.ProductControllers;
using backend.Product.Services.ProductPricingServices;

namespace backend.Product.Services.ProductServices;

/// <summary>
///     ProductMaskFieldPaths is a class which holds all the manual changes required for this API to work.
///     For instance when adding a new field or value object you would have to:
///     1. Add the new object to the AllFieldPaths (partial retrievals)
///     2. Add parsing logic for GetUpdatedProductValues (partial updates)
///     3. Add the mapping in the extension methods (TBC on making this more generic and easier)
/// </summary>
public class ProductFieldMaskConfiguration(
    IProductPricingFieldMaskService pricingFieldMaskService,
    IDimensionsFieldMaskService dimensionsFieldMaskService)
    : IProductFieldMaskConfiguration
{
    public (string name, Pricing pricing, Category category, Dimensions dimensions)
        GetUpdatedProductValues(
            UpdateProductRequest request,
            DomainModels.Product baseProduct)
    {
        string name = request.FieldMask.Contains("name", StringComparer.OrdinalIgnoreCase)
                      && !string.IsNullOrEmpty(request.Name)
            ? request.Name
            : baseProduct.Name;

        Category category = request.FieldMask.Contains("category", StringComparer.OrdinalIgnoreCase)
                            && Enum.TryParse(request.Category, true, out Category parsedCategory)
            ? parsedCategory
            : baseProduct.Category;

        Dimensions dimensions = dimensionsFieldMaskService.GetUpdatedDimensionValues(request, baseProduct.Dimensions);
        Pricing pricing = pricingFieldMaskService.GetUpdatedProductPricingValues(request, baseProduct.Pricing);

        return (name, pricing, category, dimensions);
    }

    public (
        bool isNatural,
        bool isHypoAllergenic,
        string usageInstructions,
        bool isCrueltyFree,
        string safetyWarnings
        ) GetUpdatedGroomingAndHygieneValues(
            UpdateProductRequest request,
            GroomingAndHygiene groomingAndHygiene)
    {
        bool isNatural = request.FieldMask.Contains("isnatural", StringComparer.OrdinalIgnoreCase) &&
                         request.IsNatural.HasValue
            ? request.IsNatural.Value
            : groomingAndHygiene.IsNatural;

        bool isHypoAllergenic = request.FieldMask.Contains("ishypoallergenic", StringComparer.OrdinalIgnoreCase) &&
                                request.IsHypoAllergenic.HasValue
            ? request.IsHypoAllergenic.Value
            : groomingAndHygiene.IsHypoallergenic;

        string? usageInstructions = request.FieldMask.Contains("usageinstructions", StringComparer.OrdinalIgnoreCase)
                                    && !string.IsNullOrEmpty(request.UsageInstructions)
            ? request.UsageInstructions
            : groomingAndHygiene.UsageInstructions;

        bool isCrueltyFree = request.FieldMask.Contains("iscrueltyfree", StringComparer.OrdinalIgnoreCase) &&
                             request.IsCrueltyFree.HasValue
            ? request.IsCrueltyFree.Value
            : groomingAndHygiene.IsCrueltyFree;

        string? safetyWarnings = request.FieldMask.Contains("safetywarnings", StringComparer.OrdinalIgnoreCase)
                                 && !string.IsNullOrEmpty(request.SafetyWarnings)
            ? request.SafetyWarnings
            : groomingAndHygiene.SafetyWarnings;

        return (isNatural, isHypoAllergenic, usageInstructions, isCrueltyFree, safetyWarnings);
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
        AgeGroup ageGroup =
            request.FieldMask.Contains("agegroup", StringComparer.OrdinalIgnoreCase) &&
            Enum.TryParse(request.AgeGroup, true, out AgeGroup parsedAgeGroup)
                ? parsedAgeGroup
                : petFood.AgeGroup;

        BreedSize breedSize =
            request.FieldMask.Contains("breedsize", StringComparer.OrdinalIgnoreCase) &&
            Enum.TryParse(request.BreedSize, true, out BreedSize parsedBreedSize)
                ? parsedBreedSize
                : petFood.BreedSize;

        string? ingredients =
            request.FieldMask.Contains("ingredients", StringComparer.OrdinalIgnoreCase) &&
            !string.IsNullOrEmpty(request.Ingredients)
                ? request.Ingredients
                : petFood.Ingredients;

        Dictionary<string, object> nutritionalInfo =
            request.FieldMask.Contains("nutritionalinfo", StringComparer.OrdinalIgnoreCase) &&
            request.NutritionalInfo is { } parsedNutritionalInfo
                ? parsedNutritionalInfo
                : petFood.NutritionalInfo;

        string? storageInstructions =
            request.FieldMask.Contains("storageinstructions", StringComparer.OrdinalIgnoreCase) &&
            !string.IsNullOrEmpty(request.StorageInstructions)
                ? request.StorageInstructions
                : petFood.StorageInstructions;

        decimal weight =
            request.FieldMask.Contains("weightkg", StringComparer.OrdinalIgnoreCase) &&
            !string.IsNullOrEmpty(request.WeightKg)
                ? decimal.Parse(request.WeightKg)
                : petFood.WeightKg;

        return (ageGroup, breedSize, ingredients, nutritionalInfo, storageInstructions, weight);
    }
}
