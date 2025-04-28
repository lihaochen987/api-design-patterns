using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.DomainModels;

public record PetFood : Product
{
    public required AgeGroup AgeGroup { get; init; }
    public required BreedSize BreedSize { get; init; }
    public required Ingredients Ingredients { get; init; }
    public required Dictionary<string, object> NutritionalInfo { get; init; }
    public required StorageInstructions StorageInstructions { get; init; }
    public required Weight WeightKg { get; init; }
}
