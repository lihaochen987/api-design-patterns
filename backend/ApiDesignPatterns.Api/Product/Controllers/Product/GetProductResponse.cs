using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using backend.Product.DomainModels.ValueObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.Controllers.Product;

[SwaggerDiscriminator("category")]
[JsonDerivedType(typeof(GetPetFoodResponse), "PetFood")]
[JsonDerivedType(typeof(GetGroomingAndHygieneResponse), "GroomingAndHygiene")]
public record GetProductResponse
{
    [Required] public required string Id { get; init; }
    [Required] public required string Name { get; init; }
    [Required] public required string Price { get; init; }
    [Required] public required string Category { get; init; }
    [Required] public required DimensionsResponse Dimensions { get; init; }
}

public record GetGroomingAndHygieneResponse : GetProductResponse
{
    [Required] public required bool IsNatural { get; init; }
    [Required] public required bool IsHypoAllergenic { get; init; }
    [Required] public required string UsageInstructions { get; init; }
    [Required] public required bool IsCrueltyFree { get; init; }
    [Required] public required string SafetyWarnings { get; init; }
}

public record GetPetFoodResponse : GetProductResponse
{
    [Required] public required string AgeGroup { get; init; }
    [Required] public required string BreedSize { get; init; }
    [Required] public required string Ingredients { get; init; }
    [Required] public required Dictionary<string, string> NutritionalInfo { get; init; } = new();
    [Required] public required string StorageInstructions { get; init; }
    [Required] public required string WeightKg { get; init; }
}
