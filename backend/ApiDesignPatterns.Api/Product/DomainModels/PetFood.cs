using backend.Product.DomainModels.Enums;

namespace backend.Product.DomainModels;

public record PetFood : Product
{
    public required AgeGroup AgeGroup { get; init; }
    public required BreedSize BreedSize { get; init; }
    public required string Ingredients { get; init; }
    public required Dictionary<string, object> NutritionalInfo { get; init; }
    public required string StorageInstructions { get; init; }
    public required decimal WeightKg { get; init; }
}
