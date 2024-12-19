using backend.Product.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[SwaggerDiscriminator("category")]
[SwaggerSubType(typeof(GetPetFoodResponse))]
[SwaggerSubType(typeof(GetGroomingAndHygieneResponse))]
public record GetProductResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Price { get; init; }
    public required string Category { get; init; }
    public required DimensionsResponse Dimensions { get; init; }
}

public record GetGroomingAndHygieneResponse : GetProductResponse
{
    public required bool IsNatural { get; init; }
    public required bool IsHypoAllergenic { get; init; }
    public required string UsageInstructions { get; init; }
    public required bool IsCrueltyFree { get; init; }
    public required string SafetyWarnings { get; init; }
}

public record GetPetFoodResponse : GetProductResponse
{
    public required string AgeGroup { get; init; }
    public required string BreedSize { get; init; }
    public required string Ingredients { get; init; }
    public required Dictionary<string, string> NutritionalInfo { get; init; } = new();
    public required string StorageInstructions { get; init; }
    public required string WeightKg { get; init; }
}
