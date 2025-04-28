// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.InfrastructureLayer.Database.Product;
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
        var updatedBaseProduct = UpdateBaseProduct(command.Request, command.Product);
        long id = await repository.UpdateProductAsync(updatedBaseProduct);
        switch (updatedBaseProduct)
        {
            case PetFood petFood:
                var updatedPetFood = UpdatePetFood(id, command.Request, petFood);
                await repository.UpdatePetFoodProductAsync(updatedPetFood);
                break;
            case GroomingAndHygiene groomingAndHygiene:
                var updatedGroomingAndHygiene = UpdateGroomingAndHygiene(id, command.Request, groomingAndHygiene);
                await repository.UpdateGroomingAndHygieneProductAsync(updatedGroomingAndHygiene);
                break;
        }
    }

    /// <summary>
    /// Updates base product properties if specified in field mask.
    /// </summary>
    private static DomainModels.Product UpdateBaseProduct(
        UpdateProductRequest request,
        DomainModels.Product product)
    {
        (Name name, Pricing pricing, Category category, Dimensions dimensions) =
            GetUpdatedProductValues(request, product);

        // Use the 'with' expression on the original product to maintain its type
        var updatedProduct = product with
        {
            Name = name, Category = category, Pricing = pricing, Dimensions = dimensions
        };

        return updatedProduct;
    }

    /// <summary>
    /// Updates pet food specific properties if specified in field mask.
    /// </summary>
    private static PetFood UpdatePetFood(
        long id,
        UpdateProductRequest request,
        PetFood petFood)
    {
        (AgeGroup ageGroup, BreedSize breedSize, string ingredients, Dictionary<string, object> nutritionalInfo,
                string storageInstructions, Weight weightKg) =
            GetUpdatedPetFoodValues(request, petFood);

        var updatedPetFood = petFood with
        {
            Id = id,
            AgeGroup = ageGroup,
            BreedSize = breedSize,
            Ingredients = ingredients,
            NutritionalInfo = nutritionalInfo,
            StorageInstructions = storageInstructions,
            WeightKg = weightKg
        };

        return updatedPetFood;
    }

    /// <summary>
    /// Updates pet food specific properties if specified in field mask.
    /// </summary>
    private static GroomingAndHygiene UpdateGroomingAndHygiene(
        long id,
        UpdateProductRequest request,
        GroomingAndHygiene groomingAndHygiene)
    {
        (bool isNatural, bool isHypoAllergenic, string usageInstructions, bool isCrueltyFree,
                string safetyWarnings) =
            GetUpdatedGroomingAndHygieneValues(request, groomingAndHygiene);

        var updatedGroomingAndHygiene = groomingAndHygiene with
        {
            Id = id,
            IsNatural = isNatural,
            IsHypoallergenic = isHypoAllergenic,
            UsageInstructions = usageInstructions,
            IsCrueltyFree = isCrueltyFree,
            SafetyWarnings = safetyWarnings
        };

        return updatedGroomingAndHygiene;
    }

    /// <summary>
    /// Returns updated base product values based on field mask.
    /// </summary>
    private static (Name name, Pricing pricing, Category category, Dimensions dimensions)
        GetUpdatedProductValues(
            UpdateProductRequest request,
            DomainModels.Product baseProduct)
    {
        Name name = request.FieldMask.Contains("name", StringComparer.OrdinalIgnoreCase)
                      && !string.IsNullOrEmpty(request.Name)
            ? new Name(request.Name)
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
        decimal basePrice = request.FieldMask.Contains("baseprice", StringComparer.OrdinalIgnoreCase) &&
                             request.Pricing is { BasePrice: not null }
            ? request.Pricing.BasePrice ?? product.BasePrice
            : product.BasePrice;

        decimal discountPercentage =
            request.FieldMask.Contains("discountpercentage", StringComparer.OrdinalIgnoreCase) &&
            request.Pricing is { DiscountPercentage: not null }
                ? request.Pricing.DiscountPercentage ?? product.DiscountPercentage
                : product.DiscountPercentage;

        decimal taxRate = request.FieldMask.Contains("taxrate", StringComparer.OrdinalIgnoreCase) &&
                          request.Pricing is { TaxRate: not null }
            ? request.Pricing.TaxRate ?? product.TaxRate
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
                         && request.Dimensions is {Length: not null}
            ? request.Dimensions.Length ?? currentDimensions.Length
            : currentDimensions.Length;

        decimal width = request.FieldMask.Contains("dimensions.width", StringComparer.OrdinalIgnoreCase)
                        && request.Dimensions is {Width: not null}
            ? request.Dimensions.Width ?? currentDimensions.Width
            : currentDimensions.Width;

        decimal height = request.FieldMask.Contains("dimensions.height", StringComparer.OrdinalIgnoreCase)
                         && request.Dimensions is {Height: not null}
            ? request.Dimensions.Height ?? currentDimensions.Height
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
        Weight weightKg)
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

        Weight weight = request.FieldMask.Contains("weightkg", StringComparer.OrdinalIgnoreCase)
                        && request.WeightKg.HasValue
            ? new Weight(request.WeightKg.Value)
            : petFood.WeightKg;

        return (ageGroup, breedSize, ingredients, nutritionalInfo, storageInstructions, weight);
    }
}
