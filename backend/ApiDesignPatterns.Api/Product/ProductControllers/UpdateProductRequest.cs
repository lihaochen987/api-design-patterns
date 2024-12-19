using backend.Product.Contracts;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.ProductControllers;

public record UpdateProductRequest
{
    public string? Name { get; init; }

    public ProductPricingRequest? Pricing { get; init; }

    public string? Category { get; init; }

    public DimensionsRequest? Dimensions { get; init; }

    public string? AgeGroup { get; init; }
    public string? BreedSize { get; init; }
    public string? Ingredients { get; init; }
    public Dictionary<string, object>? NutritionalInfo { get; init; }
    public string? StorageInstructions { get; init; }
    public WeightKgRequest? WeightKg { get; init; }
    public bool? IsNatural { get; init; }
    public bool? IsHypoAllergenic { get; init; }
    public string? UsageInstructions { get; init; }
    public bool? IsCrueltyFree { get; init; }
    public string? SafetyWarnings { get; init; }
    public List<string> FieldMask { get; init; } = ["*"];
}
