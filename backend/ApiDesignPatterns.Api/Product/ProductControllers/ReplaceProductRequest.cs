using System.ComponentModel.DataAnnotations;
using backend.Product.Contracts;

namespace backend.Product.ProductControllers;

public class ReplaceProductRequest
{
    [Required] public required string Name { get; set; }

    [Required] public required ProductPricingRequest Pricing { get; set; }

    [Required] public required string Category { get; set; }
    [Required] public required DimensionsRequest Dimensions { get; set; }

    public string? AgeGroup { get; set; }
    public string? BreedSize { get; set; }
    public string? Ingredients { get; set; }
    public Dictionary<string, object>? NutritionalInfo { get; set; }
    public string? StorageInstructions { get; set; }
    public string? WeightKg { get; set; }
    public bool? IsNatural { get; set; }
    public bool? IsHypoAllergenic { get; set; }
    public string? UsageInstructions { get; set; }
    public bool? IsCrueltyFree { get; set; }
    public string? SafetyWarnings { get; set; }
}
