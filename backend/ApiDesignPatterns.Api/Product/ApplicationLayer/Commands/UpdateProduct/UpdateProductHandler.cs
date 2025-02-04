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
/// Handles product update operations for the pet store product catalog, supporting different product types like PetFood and GroomingAndHygiene.
/// </summary>
/// <remarks>
/// The handler uses a field mask approach to selectively update only the specified properties,
/// maintaining existing values for unspecified fields.
/// </remarks>
/// <example>
/// Input:
/// <code>
/// // Existing product state:
/// {
///   "id": 1,
///   "name": "Basic Dog Food",
///   "weightKg": 2.5,
///   "ingredients": "Chicken, Wheat"
/// }
///
/// // Update request:
/// {
///   "name": "Premium Dog Food",
///   "fieldMask": ["name", "weightKg", "ingredients"],
///   "weightKg": "5.5",
///   "ingredients": "Chicken, Rice, Vegetables"
/// }
/// </code>
///
/// Output:
/// <code>
/// {
///   "id": 1,
///   "name": "Premium Dog Food",
///   "weightKg": 5.5,
///   "ingredients": "Chicken, Rice, Vegetables"
/// }
/// </code>
/// </example>
public class UpdateProductHandler(
    IProductRepository repository)
    : ICommandHandler<UpdateProductQuery>
{
    /// <summary>
    /// Processes the update product command by applying changes based on the field mask.
    /// </summary>
    /// <param name="command">The update command containing the request and target product.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// The method first updates base product properties, then handles type-specific updates
    /// based on whether the product is PetFood or GroomingAndHygiene.
    /// </remarks>
    public async Task Handle(UpdateProductQuery command)
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
    /// Updates the base properties of a product entity.
    /// </summary>
    /// <param name="request">The update request containing new values.</param>
    /// <param name="product">The product to update.</param>
    /// <remarks>
    /// Updates name, pricing, category, and dimensions if specified in the field mask.
    /// </remarks>
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
    /// Updates properties specific to pet food products.
    /// </summary>
    /// <param name="request">The update request containing new values.</param>
    /// <param name="petFood">The pet food product to update.</param>
    /// <remarks>
    /// Updates age group, breed size, ingredients, nutritional info, storage instructions, and weight
    /// if specified in the field mask.
    /// </remarks>
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
    /// Updates properties specific to grooming and hygiene products.
    /// </summary>
    /// <param name="request">The update request containing new values.</param>
    /// <param name="groomingAndHygiene">The grooming and hygiene product to update.</param>
    /// <remarks>
    /// Updates natural status, hypoallergenic status, usage instructions, cruelty-free status,
    /// and safety warnings if specified in the field mask.
    /// </remarks>
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
    /// Retrieves updated values for base product properties based on the field mask.
    /// </summary>
    /// <example>
    /// Input:
    /// <code>
    /// // Existing product:
    /// {
    ///   "name": "Basic Shampoo",
    ///   "category": "Grooming",
    ///   "pricing": {
    ///     "basePrice": 9.99,
    ///     "discountPercentage": 0,
    ///     "taxRate": 8.5
    ///   }
    /// }
    ///
    /// // Update request:
    /// {
    ///   "name": "Premium Shampoo",
    ///   "fieldMask": ["name", "pricing.basePrice"],
    ///   "pricing": {
    ///     "basePrice": "14.99"
    ///   }
    /// }
    /// </code>
    ///
    /// Output:
    /// <code> 
    /// {
    ///   "name": "Premium Shampoo",
    ///   "category": "Grooming",
    ///   "pricing": {
    ///     "basePrice": 14.99,
    ///     "discountPercentage": 0,
    ///     "taxRate": 8.5
    ///   }
    /// }
    /// </code>
    /// </example>
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
    /// Retrieves updated pricing values based on the field mask.
    /// </summary>
    /// <param name="request">The update request containing new values.</param>
    /// <param name="product">The current pricing state.</param>
    /// <returns>A new Pricing object with updated values.</returns>
    /// <example>
    /// <code>
    /// var updatedPricing = GetUpdatedProductPricingValues(
    ///     new UpdateProductRequest
    ///     {
    ///         FieldMask = new[] { "basePrice", "discountPercentage" },
    ///         Pricing = new PricingRequest
    ///         {
    ///             BasePrice = "29.99",
    ///             DiscountPercentage = "10.0"
    ///         }
    ///     },
    ///     existingPricing
    /// );
    /// </code>
    /// </example>
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
    /// Retrieves updated dimension values based on the field mask.
    /// </summary>
    /// <param name="request">The update request containing new values.</param>
    /// <param name="currentDimensions">The current dimensions state.</param>
    /// <returns>A new Dimensions object with updated values.</returns>
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
    /// Retrieves updated values for grooming and hygiene products based on the field mask.
    /// </summary>
    /// <param name="request">The update request containing new values.</param>
    /// <param name="groomingAndHygiene">The current grooming and hygiene product state.</param>
    /// <returns>A tuple containing updated grooming and hygiene specific values.</returns>
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
    /// Retrieves updated values for pet food products based on the field mask.
    /// </summary>
    /// <example>
    /// Input:
    /// <code>
    /// // Existing pet food:
    /// {
    ///   "ageGroup": "Puppy",
    ///   "breedSize": "Small",
    ///   "ingredients": "Chicken, Rice",
    ///   "weightKg": 1.5,
    ///   "nutritionalInfo": {
    ///     "protein": "22%",
    ///     "fat": "14%"
    ///   }
    /// }
    ///
    /// // Update request:
    /// {
    ///   "ageGroup": "Adult",
    ///   "ingredients": "Beef, Brown Rice, Carrots",
    ///   "fieldMask": ["ageGroup", "ingredients"]
    /// }
    /// </code>
    ///
    /// Output:
    /// <code>
    /// {
    ///   "ageGroup": "Adult",
    ///   "breedSize": "Small",
    ///   "ingredients": "Beef, Brown Rice, Carrots",
    ///   "weightKg": 1.5,
    ///   "nutritionalInfo": {
    ///     "protein": "22%",
    ///     "fat": "14%"
    ///   }
    /// }
    /// </code>
    /// </example>
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
