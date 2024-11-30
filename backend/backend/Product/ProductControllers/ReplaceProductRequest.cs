using backend.Product.Contracts;

namespace backend.Product.ProductControllers;

public class ReplaceProductRequest
{
    public string Name { get; set; } = "";

    public ProductPricingContract Pricing { get; set; } =
        new() { BasePrice = "", DiscountPercentage = "", TaxRate = "" };

    public string Category { get; set; } = "";
    public DimensionsContract Dimensions { get; set; } = new() { Length = "", Width = "", Height = "" };

    public string? AgeGroup { get; set; }
    public string? BreedSize { get; set; }
    public string? Ingredients { get; set; }
    public string? NutritionalInfo { get; set; }
    public string? StorageInstructions { get; set; }
    public string? WeightKg { get; set; }
}