using System.ComponentModel.DataAnnotations;
using backend.Product.Contracts;

namespace backend.Product.ProductControllers;

public class CreateProductResponse
{
    [Required] public string Name { get; set; } = "";

    [Required]
    public ProductPricingContract Pricing { get; set; } =
        new() { BasePrice = "", DiscountPercentage = "", TaxRate = "" };

    [Required] public string Category { get; set; } = "";

    [Required] public DimensionsContract Dimensions { get; set; } = new() { Length = "", Width = "", Height = "" };
}

public class CreateGroomingAndHygieneResponse
{
    [Required] public required CreateProductResponse GetProductResponse { get; set; }
    [Required] public bool? IsNatural { get; set; }
    [Required] public bool? IsHypoAllergenic { get; set; }
    [Required] public string? UsageInstructions { get; set; }
    [Required] public bool? IsCrueltyFree { get; set; }
    [Required] public string? SafetyWarnings { get; set; }
}

public class CreatePetFoodResponse
{
    [Required] public required CreateProductResponse GetProductResponse { get; set; }
    [Required] public string? AgeGroup { get; set; }
    [Required] public string? BreedSize { get; set; }
    [Required] public string? Ingredients { get; set; }
    [Required] public string? NutritionalInfo { get; set; }
    [Required] public string? StorageInstructions { get; set; }
    [Required] public string? WeightKg { get; set; }
}
