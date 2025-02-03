using backend.Product.DomainModels.Enums;

namespace backend.Product.DomainModels;

public class PetFood : Product
{
    public required AgeGroup AgeGroup { get; set; }
    public required BreedSize BreedSize { get; set; }
    public required string Ingredients { get; set; }
    public required Dictionary<string, object> NutritionalInfo { get; set; }
    public required string StorageInstructions { get; set; }
    public required decimal WeightKg { get; set; }
}
