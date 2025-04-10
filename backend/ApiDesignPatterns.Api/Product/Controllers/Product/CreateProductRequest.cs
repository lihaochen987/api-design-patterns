using System.ComponentModel.DataAnnotations;
using backend.Product.DomainModels.ValueObjects;
using backend.Shared.Caching;

namespace backend.Product.Controllers.Product;

public record CreateProductRequest : ICachedCommand
{
    public string? RequestId { get; init; }
    [Required] public required string Name { get; init; }
    [Required] public required ProductPricingRequest Pricing { get; init; }
    [Required] public required string Category { get; init; }
    [Required] public required DimensionsRequest Dimensions { get; init; }
    public string? AgeGroup { get; init; }
    public string? BreedSize { get; init; }
    public string? Ingredients { get; init; }
    public Dictionary<string, object>? NutritionalInfo { get; init; }
    public string? StorageInstructions { get; init; }
    public decimal? WeightKg { get; init; }
    public bool? IsNatural { get; init; }
    public bool? IsHypoAllergenic { get; init; }
    public string? UsageInstructions { get; init; }
    public bool? IsCrueltyFree { get; init; }
    public string? SafetyWarnings { get; init; }
}
