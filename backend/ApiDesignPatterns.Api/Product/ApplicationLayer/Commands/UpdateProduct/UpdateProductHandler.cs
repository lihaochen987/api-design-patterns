// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.InfrastructureLayer.Database.Product;
using backend.Product.ProductControllers;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.Commands.UpdateProduct;

/// <summary>
/// Updates pet store product catalog items using field masking to selectively modify properties.
/// </summary>
/// <example>
/// Update request:
/// {
///   "name": "Premium Dog Food",
///   "fieldMask": ["name", "weightKg"],
///   "weightKg": "5.5"
/// }
/// </example>
public class UpdateProductHandler(
    IProductRepository repository)
    : ICommandHandler<UpdateProductCommand>
{
    /// <summary>
    /// Updates product properties specified in the field mask.
    /// </summary>
    public async Task Handle(UpdateProductCommand command)
    {
        UpdateBaseProduct(command.Request, command.Product);
        long id = await repository.UpdateProductAsync(command.Product);
        command.Product.Id = id;
        switch (command.Product)
        {
            case PetFood petFood:
                UpdatePetFood(command.Request, petFood);
                await repository.UpdatePetFoodProductAsync(petFood);
                break;
            case GroomingAndHygiene groomingAndHygiene:
                UpdateGroomingAndHygiene(command.Request, groomingAndHygiene);
                await repository.UpdateGroomingAndHygieneProductAsync(groomingAndHygiene);
                break;
        }
    }

    /// <summary>
    /// Updates base product properties if specified in field mask.
    /// </summary>
    private static void UpdateBaseProduct(
        UpdateProductRequest request,
        DomainModels.Product product)
    {
        (string name, Pricing pricing, Category category, Dimensions dimensions) =
            GetUpdatedProductValues(request, product);

        product.Name = name;
        product.Pricing = pricing;
        product.Category = category;
        product.Dimensions = dimensions;
    }

    /// <summary>
    /// Updates pet food specific properties if specified in field mask.
    /// </summary>
    private static void UpdatePetFood(
        UpdateProductRequest request,
        PetFood petFood)
    {
        (AgeGroup ageGroup, BreedSize breedSize, string ingredients, Dictionary<string, object> nutritionalInfo,
                string storageInstructions, decimal weightKg) =
            GetUpdatedPetFoodValues(request, petFood);

        petFood.AgeGroup = ageGroup;
        petFood.BreedSize = breedSize;
        petFood.Ingredients = ingredients;
        petFood.NutritionalInfo = nutritionalInfo;
        petFood.StorageInstructions = storageInstructions;
        petFood.WeightKg = weightKg;
    }

    /// <summary>
    /// Updates pet food specific properties if specified in field mask.
    /// </summary>
    private static void UpdateGroomingAndHygiene(
        UpdateProductRequest request,
        GroomingAndHygiene groomingAndHygiene)
    {
        (bool isNatural, bool isHypoAllergenic, string usageInstructions, bool isCrueltyFree,
                string safetyWarnings) =
            GetUpdatedGroomingAndHygieneValues(request, groomingAndHygiene);

        groomingAndHygiene.IsNatural = isNatural;
        groomingAndHygiene.IsHypoallergenic = isHypoAllergenic;
        groomingAndHygiene.UsageInstructions = usageInstructions;
        groomingAndHygiene.IsCrueltyFree = isCrueltyFree;
        groomingAndHygiene.SafetyWarnings = safetyWarnings;
    }

    /// <summary>
    /// Returns updated base product values based on field mask.
    /// </summary>
    private static (string name, Pricing pricing, Category category, Dimensions dimensions)
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

        Dimensions dimensions = GetUpdatedDimensionValues(request, baseProduct.Dimensions);
        Pricing pricing = GetUpdatedProductPricingValues(request, baseProduct.Pricing);

        return (name, pricing, category, dimensions);
    }

    /// <summary>
    /// Returns updated pricing values based on field mask.
    /// </summary>
    private static Pricing
        GetUpdatedProductPricingValues(
            UpdateProductRequest request,
            Pricing product)
    {
        decimal basePrice = request.FieldMask.Contains("baseprice", StringComparer.OrdinalIgnoreCase)
                            && decimal.TryParse(request.Pricing?.BasePrice, out decimal parsedBasePrice)
            ? parsedBasePrice
            : product.BasePrice;

        decimal discountPercentage = request.FieldMask.Contains("discountpercentage", StringComparer.OrdinalIgnoreCase)
                                     && decimal.TryParse(request.Pricing?.DiscountPercentage,
                                         out decimal parsedDiscountPercentage)
            ? parsedDiscountPercentage
            : product.DiscountPercentage;

        decimal taxRate = request.FieldMask.Contains("taxrate", StringComparer.OrdinalIgnoreCase)
                          && decimal.TryParse(request.Pricing?.TaxRate, out decimal parsedTaxRate)
            ? parsedTaxRate
            : product.TaxRate;

        return new Pricing(basePrice, discountPercentage, taxRate);
    }

    /// <summary>
    /// Returns updated dimension values based on field mask.
    /// </summary>
    private static Dimensions GetUpdatedDimensionValues(
        UpdateProductRequest request,
        Dimensions currentDimensions)
    {
        decimal length = request.FieldMask.Contains("dimensions.length", StringComparer.OrdinalIgnoreCase)
                         && !string.IsNullOrEmpty(request.Dimensions?.Length)
            ? decimal.Parse(request.Dimensions.Length)
            : currentDimensions.Length;

        decimal width = request.FieldMask.Contains("dimensions.width", StringComparer.OrdinalIgnoreCase)
                        && !string.IsNullOrEmpty(request.Dimensions?.Width)
            ? decimal.Parse(request.Dimensions.Width)
            : currentDimensions.Width;

        decimal height = request.FieldMask.Contains("dimensions.height", StringComparer.OrdinalIgnoreCase)
                         && !string.IsNullOrEmpty(request.Dimensions?.Height)
            ? decimal.Parse(request.Dimensions.Height)
            : currentDimensions.Height;

        return new Dimensions(length, width, height);
    }

    /// <summary>
    /// Returns updated grooming and hygiene values based on field mask.
    /// </summary>
    private static (
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


    /// <summary>
    /// Returns updated pet food values based on field mask.
    /// </summary>
    private static (
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
