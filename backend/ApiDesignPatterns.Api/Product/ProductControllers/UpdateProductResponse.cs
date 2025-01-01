using backend.Product.DomainModels.ValueObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[SwaggerDiscriminator("category")]
[SwaggerSubType(typeof(UpdatePetFoodResponse))]
[SwaggerSubType(typeof(UpdateGroomingAndHygieneResponse))]
public record UpdateProductResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required ProductPricingResponse Pricing { get; init; }
    public required string Category { get; init; }
    public required DimensionsResponse Dimensions { get; init; }
}

public record UpdateGroomingAndHygieneResponse : UpdateProductResponse
{
    public required bool IsNatural { get; init; }
    public required bool IsHypoAllergenic { get; init; }
    public required string UsageInstructions { get; init; }
    public required bool IsCrueltyFree { get; init; }
    public required string SafetyWarnings { get; init; }
}

public record UpdatePetFoodResponse : UpdateProductResponse
{
    public required string AgeGroup { get; init; }
    public required string BreedSize { get; init; }
    public required string Ingredients { get; init; }
    public required string NutritionalInfo { get; init; }
    public required string StorageInstructions { get; init; }
    public required string WeightKg { get; init; }
}
