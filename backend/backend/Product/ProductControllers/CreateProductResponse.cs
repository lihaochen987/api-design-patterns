using System.ComponentModel.DataAnnotations;
using backend.Product.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[SwaggerDiscriminator("category")]
[SwaggerSubType(typeof(CreatePetFoodResponse))]
[SwaggerSubType(typeof(CreateGroomingAndHygieneResponse))]
public class CreateProductResponse
{
    [Required] public required string Id { get; init; }
    [Required] public required string Name { get; init; }

    [Required] public required ProductPricingContract Pricing { get; init; }

    [Required] public required string Category { get; init; }

    [Required] public required DimensionsContract Dimensions { get; init; }
}

public class CreateGroomingAndHygieneResponse : CreateProductResponse
{
    [Required] public required bool IsNatural { get; init; }
    [Required] public required bool IsHypoAllergenic { get; init; }
    [Required] public required string UsageInstructions { get; init; }
    [Required] public required bool IsCrueltyFree { get; init; }
    [Required] public required string SafetyWarnings { get; init; }
}

public class CreatePetFoodResponse : CreateProductResponse
{
    [Required] public required string AgeGroup { get; init; }
    [Required] public required string BreedSize { get; init; }
    [Required] public required string Ingredients { get; init; }
    [Required] public required string NutritionalInfo { get; init; }
    [Required] public required string StorageInstructions { get; init; }
    [Required] public required string WeightKg { get; init; }
}
