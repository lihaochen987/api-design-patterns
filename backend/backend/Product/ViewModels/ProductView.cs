using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.ViewModels;

public class ProductView
{
    public long Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public Category Category { get; init; }
    public Dimensions Dimensions { get; init; }
    public AgeGroup? AgeGroup { get; init; }
    public BreedSize? BreedSize { get; init; }
    public string? Ingredients { get; init; }
    public Dictionary<string, object>? NutritionalInfo { get; init; }
    public string? StorageInstructions { get; init; }
    public decimal? WeightKg { get; init; }
    public bool? IsNatural { get; init; }
    public bool? IsHypoallergenic { get; init; }
    public string? UsageInstructions { get; init; }
    public bool? IsCrueltyFree { get; init; }
    public string? SafetyWarnings { get; init; }
}
