using System.ComponentModel.DataAnnotations;
using backend.Product.DomainModels.Enums;

namespace backend.Product.DomainModels;

public class PetFood : Product
{
    [Required] public required AgeGroup AgeGroup { get; set; }
    [Required] public required BreedSize BreedSize { get; set; }
    [Required] [MaxLength(300)] public required string Ingredients { get; set; }
    public required Dictionary<string, object> NutritionalInfo { get; set; }
    [Required] [MaxLength(300)] public required string StorageInstructions { get; set; }
    [Required] [Range(0.01, 1000)] public required decimal WeightKg { get; set; }
}
