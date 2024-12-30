// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.ProductControllers;

namespace backend.Product.Services;

public class UpdateProductService(IProductFieldMaskConfiguration maskConfiguration)
{
    public void UpdateBaseProduct(
        UpdateProductRequest request,
        DomainModels.Product product)
    {
        (string name, Pricing pricing, Category category, Dimensions dimensions) =
            maskConfiguration.GetUpdatedProductValues(request, product);

        product.Name = name;
        product.Pricing = pricing;
        product.Category = category;
        product.Dimensions = dimensions;
    }

    public void UpdatePetFood(
        UpdateProductRequest request,
        PetFood petFood)
    {
        (AgeGroup ageGroup, BreedSize breedSize, string ingredients, Dictionary<string, object> nutritionalInfo,
                string storageInstructions, decimal weightKg) =
            maskConfiguration.GetUpdatedPetFoodValues(request, petFood);

        petFood.AgeGroup = ageGroup;
        petFood.BreedSize = breedSize;
        petFood.Ingredients = ingredients;
        petFood.NutritionalInfo = nutritionalInfo;
        petFood.StorageInstructions = storageInstructions;
        petFood.WeightKg = weightKg;
    }

    public void UpdateGroomingAndHygiene(
        UpdateProductRequest request,
        GroomingAndHygiene groomingAndHygiene)
    {
        (bool isNatural, bool isHypoAllergenic, string usageInstructions, bool isCrueltyFree,
                string safetyWarnings) =
            maskConfiguration.GetUpdatedGroomingAndHygieneValues(request, groomingAndHygiene);

        groomingAndHygiene.IsNatural = isNatural;
        groomingAndHygiene.IsHypoallergenic = isHypoAllergenic;
        groomingAndHygiene.UsageInstructions = usageInstructions;
        groomingAndHygiene.IsCrueltyFree = isCrueltyFree;
        groomingAndHygiene.SafetyWarnings = safetyWarnings;
    }
}
