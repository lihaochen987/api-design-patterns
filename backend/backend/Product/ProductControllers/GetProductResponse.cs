using System.ComponentModel.DataAnnotations;
using backend.Product.Contracts;

namespace backend.Product.ProductControllers;

public class GetProductResponse
{
    [Required] public string Id { get; init; } = "";
    [Required] public string Name { get; init; } = "";
    [Required] public string Price { get; init; } = "";
    [Required] public string Category { get; init; } = "";
    [Required] public DimensionsContract Dimensions { get; init; } = new() { Length = "", Width = "", Height = "" };
}

public class GetGroomingAndHygieneResponse : GetProductResponse
{
    [Required] public bool? IsNatural { get; init; } = false;
    [Required] public bool? IsHypoAllergenic { get; init; } = false;
    [Required] public string? UsageInstructions { get; init; } = "";
    [Required] public bool? IsCrueltyFree { get; init; } = false;
    [Required] public string? SafetyWarnings { get; init; } = "";
}

public class GetPetFoodResponse : GetProductResponse
{
    [Required] public string? AgeGroup { get; set; }
    [Required] public string? BreedSize { get; set; }
    [Required] public string? Ingredients { get; set; }
    [Required] public string? NutritionalInfo { get; set; }
    [Required] public string? StorageInstructions { get; set; }
    [Required] public string? WeightKg { get; set; }
}
