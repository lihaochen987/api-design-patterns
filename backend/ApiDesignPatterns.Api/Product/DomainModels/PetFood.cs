using System.ComponentModel.DataAnnotations;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.DomainModels;

public class PetFood : Product
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private PetFood()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public PetFood(
        string name,
        Pricing pricing,
        Dimensions dimensions,
        AgeGroup ageGroup,
        BreedSize breedSize,
        string ingredients,
        Dictionary<string, object> nutritionalInfo,
        string storageInstructions,
        decimal weightKg)
        : base(name, pricing, Category.PetFood, dimensions)
    {
        AgeGroup = ageGroup;
        BreedSize = breedSize;
        Ingredients = ingredients;
        NutritionalInfo = nutritionalInfo;
        StorageInstructions = storageInstructions;
        WeightKg = weightKg;

        EnforcePetFoodInvariants(weightKg);
    }

    public AgeGroup AgeGroup { get; private set; }
    public BreedSize BreedSize { get; private set; }
    [MaxLength(300)] public string Ingredients { get; private set; }
    public Dictionary<string, object> NutritionalInfo { get; private set; }
    [MaxLength(300)] public string StorageInstructions { get; private set; }
    public decimal WeightKg { get; private set; }

    public void UpdatePetFoodDetails(
        AgeGroup ageGroup,
        BreedSize breedSize,
        string ingredients,
        Dictionary<string, object> nutritionalInfo,
        string storageInstructions,
        decimal weightKg)
    {
        EnforcePetFoodInvariants(weightKg);

        AgeGroup = ageGroup;
        BreedSize = breedSize;
        Ingredients = ingredients;
        NutritionalInfo = nutritionalInfo;
        StorageInstructions = storageInstructions;
        WeightKg = weightKg;
    }

    private static void EnforcePetFoodInvariants(decimal weightKg)
    {
        if (weightKg <= 0)
        {
            throw new ArgumentException("Weight must be greater than zero.");
        }
    }
}
