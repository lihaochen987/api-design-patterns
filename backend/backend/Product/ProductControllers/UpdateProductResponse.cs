using System.ComponentModel.DataAnnotations;
using backend.Product.Contracts;

namespace backend.Product.ProductControllers;

public class UpdateProductResponse
{
    [Required] public string Id { get; set; } = "";
    [Required] public string Name { get; set; } = "";

    [Required]
    public ProductPricingContract Pricing { get; set; } =
        new() { BasePrice = "", DiscountPercentage = "", TaxRate = "" };

    [Required] public string Category { get; set; } = "";
    [Required] public DimensionsContract Dimensions { get; set; } = new() { Length = "", Width = "", Height = "" };

    public string? AgeGroup { get; set; }
    public string? BreedSize { get; set; }
    public string? Ingredients { get; set; }
    public string? NutritionalInfo { get; set; }
    public string? StorageInstructions { get; set; }
    public string? WeightKg { get; set; }

    public bool? IsNatural { get; set; }
    public bool? IsHypoAllergenic { get; set; }
    public string? UsageInstructions { get; set; }
    public bool? IsCrueltyFree { get; set; }
    public string? SafetyWarnings { get; set; }
}
