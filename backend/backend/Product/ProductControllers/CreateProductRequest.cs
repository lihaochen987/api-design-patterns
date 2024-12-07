using System.ComponentModel.DataAnnotations;
using backend.Product.Contracts;

namespace backend.Product.ProductControllers;

public class CreateProductRequest
{
    [Required] public required string Name { get; init; }

    [Required] public required ProductPricingContract Pricing { get; init; }

    [Required] public required string Category { get; init; }

    [Required] public required DimensionsContract Dimensions { get; init; }
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
