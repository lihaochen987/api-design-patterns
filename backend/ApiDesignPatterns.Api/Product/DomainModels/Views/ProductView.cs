using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Shared;

namespace backend.Product.DomainModels.Views;

public class ProductView : Identifier
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public Category Category { get; set; }
    public required Dimensions Dimensions { get; set; }
    public AgeGroup? AgeGroup { get; set; }
    public BreedSize? BreedSize { get; set; }
    public string? Ingredients { get; set; }
    public Dictionary<string, object>? NutritionalInfo { get; set; }
    public string? StorageInstructions { get; set; }
    public decimal? WeightKg { get; set; }
    public bool? IsNatural { get; set; }
    public bool? IsHypoallergenic { get; set; }
    public string? UsageInstructions { get; set; }
    public bool? IsCrueltyFree { get; set; }
    public string? SafetyWarnings { get; set; }
}
