using System.ComponentModel.DataAnnotations;
using backend.Product.DomainModels.ValueObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[SwaggerDiscriminator("category")]
[SwaggerSubType(typeof(CreatePetFoodResponse))]
[SwaggerSubType(typeof(CreateGroomingAndHygieneResponse))]
public record CreateProductResponse
{
    [Required] public required string Id { get; init; }
    [Required] public required string Name { get; init; }
    [Required] public required ProductPricingResponse Pricing { get; init; }
    [Required] public required string Category { get; init; }
    [Required] public required DimensionsResponse Dimensions { get; init; }
}

public record CreateGroomingAndHygieneResponse : CreateProductResponse
{
    [Required] public required bool IsNatural { get; init; }
    [Required] public required bool IsHypoAllergenic { get; init; }
    [Required] public required string UsageInstructions { get; init; }
    [Required] public required bool IsCrueltyFree { get; init; }
    [Required] public required string SafetyWarnings { get; init; }
}

public record CreatePetFoodResponse : CreateProductResponse
{
    [Required] public required string AgeGroup { get; init; }
    [Required] public required string BreedSize { get; init; }
    [Required] public required string Ingredients { get; init; }
    [Required] public required string NutritionalInfo { get; init; }
    [Required] public required string StorageInstructions { get; init; }
    [Required] public required string WeightKg { get; init; }
}
