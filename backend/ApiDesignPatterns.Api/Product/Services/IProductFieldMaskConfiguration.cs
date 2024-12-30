// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.ProductControllers;

namespace backend.Product.Services;

public interface IProductFieldMaskConfiguration
{
    public (string name, Pricing pricing, Category category, Dimensions dimensions)
        GetUpdatedProductValues(
            UpdateProductRequest request,
            DomainModels.Product baseProduct);

    public (
        bool isNatural,
        bool isHypoAllergenic,
        string usageInstructions,
        bool isCrueltyFree,
        string safetyWarnings
        ) GetUpdatedGroomingAndHygieneValues(
            UpdateProductRequest request,
            GroomingAndHygiene groomingAndHygiene);

    public (
        AgeGroup ageGroup,
        BreedSize breedSize,
        string ingredients,
        Dictionary<string, object> nutritionalInfo,
        string storageInstructions,
        decimal weightKg)
        GetUpdatedPetFoodValues(
            UpdateProductRequest request,
            PetFood petFood);
}
