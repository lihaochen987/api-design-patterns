using System.ComponentModel.DataAnnotations;
using backend.Product.DomainModels.Enums;

namespace backend.Product.DomainModels;

public class PetFood : Product
{
    private decimal _weightKg;
    public AgeGroup AgeGroup { get; set; }
    public BreedSize BreedSize { get; set; }
    [MaxLength(300)] public required string Ingredients { get; set; }
    public required Dictionary<string, object> NutritionalInfo { get; set; }
    [MaxLength(300)] public required string StorageInstructions { get; set; }

    public decimal WeightKg
    {
        get => _weightKg;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException("Weight must be greater than zero.");
            }

            _weightKg = value;
        }
    }
}
