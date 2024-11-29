namespace backend.Product.DomainModels;

public class PetFood : Product
{
    public PetFood(
        long id,
        string name,
        Pricing pricing,
        Dimensions dimensions,
        string ageGroup,
        string breedSize,
        string ingredients,
        string nutritionalInfo,
        string storageInstructions,
        decimal weightKg)
        : base(id, name, pricing, Category.PetFood, dimensions)
    {
        AgeGroup = ageGroup;
        BreedSize = breedSize;
        Ingredients = ingredients;
        NutritionalInfo = nutritionalInfo;
        StorageInstructions = storageInstructions;
        WeightKg = weightKg;

        EnforcePetFoodInvariants(ageGroup, breedSize, weightKg);
    }

    public PetFood(
        string name,
        Pricing pricing,
        Dimensions dimensions,
        string ageGroup,
        string breedSize,
        string ingredients,
        string nutritionalInfo,
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

        EnforcePetFoodInvariants(ageGroup, breedSize, weightKg);
    }

    public string AgeGroup { get; private set; }
    public string BreedSize { get; private set; }
    public string Ingredients { get; private set; }
    public string NutritionalInfo { get; private set; }
    public string StorageInstructions { get; private set; }
    public decimal WeightKg { get; private set; }

    public void UpdatePetFoodDetails(
        string ageGroup,
        string breedSize,
        string ingredients,
        string nutritionalInfo,
        string storageInstructions,
        decimal weightKg)
    {
        EnforcePetFoodInvariants(ageGroup, breedSize, weightKg);

        AgeGroup = ageGroup;
        BreedSize = breedSize;
        Ingredients = ingredients;
        NutritionalInfo = nutritionalInfo;
        StorageInstructions = storageInstructions;
        WeightKg = weightKg;
    }

    private static void EnforcePetFoodInvariants(string ageGroup, string breedSize, decimal weightKg)
    {
        if (string.IsNullOrWhiteSpace(ageGroup))
            throw new ArgumentException("Age group is required for pet food.");
        if (string.IsNullOrWhiteSpace(breedSize))
            throw new ArgumentException("Breed size is required for pet food.");
        if (weightKg <= 0)
            throw new ArgumentException("Weight must be greater than zero.");
    }
}