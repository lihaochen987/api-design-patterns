using System.ComponentModel.DataAnnotations;
using backend.Product.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[SwaggerDiscriminator("category")]
[SwaggerSubType(typeof(GetPetFoodResponse))]
[SwaggerSubType(typeof(GetGroomingAndHygieneResponse))]
public class GetProductResponse
{
    [Required] public required string Id { get; init; }
    [Required] public required string Name { get; init; }
    [Required] public required string Price { get; init; }
    [Required] public required string Category { get; init; }
    [Required] public required DimensionsContract Dimensions { get; init; }
}

public class GetGroomingAndHygieneResponse : GetProductResponse
{
    [Required] public required bool IsNatural { get; init; }
    [Required] public required bool IsHypoAllergenic { get; init; }
    [Required] public required string UsageInstructions { get; init; }
    [Required] public required bool IsCrueltyFree { get; init; }
    [Required] public required string SafetyWarnings { get; init; }
}

public class GetPetFoodResponse : GetProductResponse
{
    [Required] public required string AgeGroup { get; init; }
    [Required] public required string BreedSize { get; init; }
    [Required] public required string Ingredients { get; init; }
    [Required] public required Dictionary<string, string> NutritionalInfo { get; init; } = new();
    [Required] public required string StorageInstructions { get; init; }
    [Required] public required string WeightKg { get; init; }
}
