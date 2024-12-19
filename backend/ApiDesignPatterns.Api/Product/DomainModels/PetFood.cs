using System.ComponentModel.DataAnnotations;
using backend.Product.DomainModels.Enums;

namespace backend.Product.DomainModels;

public class PetFood : Product
{
    public AgeGroup AgeGroup { get; set; }
    public BreedSize BreedSize { get; set; }
    [MaxLength(300)] public required string Ingredients { get; set; }
    public required Dictionary<string, object> NutritionalInfo { get; set; }
    [MaxLength(300)] public required string StorageInstructions { get; set; }
    [Range(0.01, 1000)] public decimal WeightKg { get; set; }
}
