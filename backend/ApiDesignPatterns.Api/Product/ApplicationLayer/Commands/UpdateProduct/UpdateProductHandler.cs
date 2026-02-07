// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

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
    IUpdateProduct repository)
    : ICommandHandler<UpdateProductCommand>
{
    /// <summary>
    /// Updates product properties specified in the field mask.
    /// </summary>
    public async Task Handle(UpdateProductCommand command)
    {
        // PURE
        var request = command.Request;
        var product = command.Product;

        string name = request.FieldMask.Contains("name", StringComparer.OrdinalIgnoreCase)
                    && !string.IsNullOrEmpty(request.Name)
            ? request.Name
            : product.Name;

        Category category = request.FieldMask.Contains("category", StringComparer.OrdinalIgnoreCase)
                            && Enum.TryParse(request.Category, true, out Category parsedCategory)
            ? parsedCategory
            : product.Category;

        decimal length = request.FieldMask.Contains("dimensions.length", StringComparer.OrdinalIgnoreCase)
                         && request.Dimensions is { Length: not null }
            ? request.Dimensions.Length ?? product.Dimensions.Length
            : product.Dimensions.Length;

        decimal width = request.FieldMask.Contains("dimensions.width", StringComparer.OrdinalIgnoreCase)
                        && request.Dimensions is { Width: not null }
            ? request.Dimensions.Width ?? product.Dimensions.Width
            : product.Dimensions.Width;

        decimal height = request.FieldMask.Contains("dimensions.height", StringComparer.OrdinalIgnoreCase)
                         && request.Dimensions is { Height: not null }
            ? request.Dimensions.Height ?? product.Dimensions.Height
            : product.Dimensions.Height;

        var dimensions = new Dimensions(length, width, height);

        decimal basePrice = request.FieldMask.Contains("baseprice", StringComparer.OrdinalIgnoreCase) &&
                            request.Pricing is { BasePrice: not null }
            ? request.Pricing.BasePrice ?? product.Pricing.BasePrice
            : product.Pricing.BasePrice;

        decimal discountPercentage =
            request.FieldMask.Contains("discountpercentage", StringComparer.OrdinalIgnoreCase) &&
            request.Pricing is { DiscountPercentage: not null }
                ? request.Pricing.DiscountPercentage ?? product.Pricing.DiscountPercentage
                : product.Pricing.DiscountPercentage;

        decimal taxRate = request.FieldMask.Contains("taxrate", StringComparer.OrdinalIgnoreCase) &&
                          request.Pricing is { TaxRate: not null }
            ? request.Pricing.TaxRate ?? product.Pricing.TaxRate
            : product.Pricing.TaxRate;

        var pricing = new Pricing(basePrice, discountPercentage, taxRate);

        var updatedBaseProduct = product with
        {
            Name = name,
            Category = category,
            Pricing = pricing,
            Dimensions = dimensions
        };

        DomainModels.Product updatedProduct = updatedBaseProduct switch
        {
            PetFood petFood => petFood with
            {
                AgeGroup = request.FieldMask.Contains("agegroup", StringComparer.OrdinalIgnoreCase) &&
                           Enum.TryParse(request.AgeGroup, true, out AgeGroup parsedAgeGroup)
                    ? parsedAgeGroup
                    : petFood.AgeGroup,
                BreedSize = request.FieldMask.Contains("breedsize", StringComparer.OrdinalIgnoreCase) &&
                            Enum.TryParse(request.BreedSize, true, out BreedSize parsedBreedSize)
                    ? parsedBreedSize
                    : petFood.BreedSize,
                Ingredients = request.FieldMask.Contains("ingredients", StringComparer.OrdinalIgnoreCase) &&
                              !string.IsNullOrEmpty(request.Ingredients)
                    ? request.Ingredients
                    : petFood.Ingredients,
                NutritionalInfo = request.FieldMask.Contains("nutritionalinfo", StringComparer.OrdinalIgnoreCase) &&
                                  request.NutritionalInfo is { } parsedNutritionalInfo
                    ? parsedNutritionalInfo
                    : petFood.NutritionalInfo,
                StorageInstructions = request.FieldMask.Contains("storageinstructions", StringComparer.OrdinalIgnoreCase) &&
                                      !string.IsNullOrEmpty(request.StorageInstructions)
                    ? new StorageInstructions(request.StorageInstructions)
                    : petFood.StorageInstructions,
                WeightKg = request.FieldMask.Contains("weightkg", StringComparer.OrdinalIgnoreCase) &&
                           request.WeightKg.HasValue
                    ? new Weight(request.WeightKg.Value)
                    : petFood.WeightKg
            },
            GroomingAndHygiene groomingAndHygiene => groomingAndHygiene with
            {
                IsNatural = request.FieldMask.Contains("isnatural", StringComparer.OrdinalIgnoreCase) &&
                            request.IsNatural.HasValue
                    ? request.IsNatural.Value
                    : groomingAndHygiene.IsNatural,
                IsHypoallergenic = request.FieldMask.Contains("ishypoallergenic", StringComparer.OrdinalIgnoreCase) &&
                                   request.IsHypoAllergenic.HasValue
                    ? request.IsHypoAllergenic.Value
                    : groomingAndHygiene.IsHypoallergenic,
                UsageInstructions = request.FieldMask.Contains("usageinstructions", StringComparer.OrdinalIgnoreCase) &&
                                    !string.IsNullOrEmpty(request.UsageInstructions)
                    ? new UsageInstructions(request.UsageInstructions)
                    : groomingAndHygiene.UsageInstructions,
                IsCrueltyFree = request.FieldMask.Contains("iscrueltyfree", StringComparer.OrdinalIgnoreCase) &&
                                request.IsCrueltyFree.HasValue
                    ? request.IsCrueltyFree.Value
                    : groomingAndHygiene.IsCrueltyFree,
                SafetyWarnings = request.FieldMask.Contains("safetywarnings", StringComparer.OrdinalIgnoreCase) &&
                                 !string.IsNullOrEmpty(request.SafetyWarnings)
                    ? request.SafetyWarnings
                    : groomingAndHygiene.SafetyWarnings
            },
            _ => updatedBaseProduct
        };

        // IMPURE
        await repository.UpdateProductAsync(updatedProduct);
    }
}
