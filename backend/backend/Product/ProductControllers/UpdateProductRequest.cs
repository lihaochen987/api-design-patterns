using backend.Product.Contracts;

namespace backend.Product.ProductControllers;

public class UpdateProductRequest
{
    public string Name { get; set; } = "";

    public ProductPricingContract Pricing { get; set; } =
        new() { BasePrice = "", DiscountPercentage = "", TaxRate = "" };

    public string Category { get; set; } = "";

    public DimensionsContract Dimensions { get; set; } = new() { Length = "", Width = "", Height = "" };

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
    public List<string> FieldMask { get; init; } = ["*"];
}
