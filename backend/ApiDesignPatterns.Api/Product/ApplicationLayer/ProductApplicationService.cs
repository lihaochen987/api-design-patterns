// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.InfrastructureLayer;
using backend.Product.ProductControllers;
using backend.Product.Services;

namespace backend.Product.ApplicationLayer;

public class ProductApplicationService(
    IProductRepository repository,
    ProductFieldMaskConfiguration maskConfiguration)
    : IProductApplicationService
{
    public async Task<DomainModels.Product?> GetProductAsync(long id)
    {
        DomainModels.Product? product = await repository.GetProductAsync(id);
        return product ?? null;
    }

    public async Task CreateProductAsync(DomainModels.Product product) =>
        await repository.CreateProductAsync(product);

    public async Task DeleteProductAsync(DomainModels.Product product) =>
        await repository.DeleteProductAsync(product);

    public async Task UpdateProductAsync(
        UpdateProductRequest request,
        DomainModels.Product product)
    {
        UpdateBaseProduct(maskConfiguration, request, product);
        switch (product)
        {
            case PetFood petFood:
                UpdatePetFood(maskConfiguration, request, petFood);
                break;
            case GroomingAndHygiene groomingAndHygiene:
                UpdateGroomingAndHygiene(maskConfiguration, request, groomingAndHygiene);
                break;
        }
        await repository.UpdateProductAsync(product);
    }

    public async Task ReplaceProductAsync(DomainModels.Product product)
    {
        await repository.UpdateProductAsync(product);
    }

    private static void UpdateBaseProduct(
        ProductFieldMaskConfiguration maskConfiguration,
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

    private static void UpdatePetFood(
        ProductFieldMaskConfiguration maskConfiguration,
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

    private static void UpdateGroomingAndHygiene(
        ProductFieldMaskConfiguration maskConfiguration,
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
