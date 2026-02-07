using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.DomainModels;

public record PetFood : Product
{
    public required AgeGroup AgeGroup { get; init; }
    public required BreedSize BreedSize { get; init; }
    public required string Ingredients { get; init; }
    public required Dictionary<string, object> NutritionalInfo { get; init; }
    public required string StorageInstructions { get; init; }
    public required Weight WeightKg { get; init; }

    public override Product ApplyUpdates(UpdateProductRequest request)
    {
        var baseProduct = base.ApplyUpdates(request);

        var ageGroup = request.FieldMask.Contains("agegroup", StringComparer.OrdinalIgnoreCase) &&
                       Enum.TryParse(request.AgeGroup, true, out AgeGroup parsedAgeGroup)
            ? parsedAgeGroup
            : AgeGroup;

        var breedSize = request.FieldMask.Contains("breedsize", StringComparer.OrdinalIgnoreCase) &&
                        Enum.TryParse(request.BreedSize, true, out BreedSize parsedBreedSize)
            ? parsedBreedSize
            : BreedSize;

        string? ingredients = request.FieldMask.Contains("ingredients", StringComparer.OrdinalIgnoreCase) &&
                              !string.IsNullOrEmpty(request.Ingredients)
            ? request.Ingredients
            : Ingredients;

        var nutritionalInfo = request.FieldMask.Contains("nutritionalinfo", StringComparer.OrdinalIgnoreCase) &&
                              request.NutritionalInfo is { } parsedNutritionalInfo
            ? parsedNutritionalInfo
            : NutritionalInfo;

        string? storageInstructions = request.FieldMask.Contains("storageinstructions", StringComparer.OrdinalIgnoreCase) &&
                                      !string.IsNullOrEmpty(request.StorageInstructions)
            ? request.StorageInstructions
            : StorageInstructions;

        var weightKg = request.FieldMask.Contains("weightkg", StringComparer.OrdinalIgnoreCase) &&
                       request.WeightKg.HasValue
            ? new Weight(request.WeightKg.Value)
            : WeightKg;

        return (baseProduct as PetFood)! with
        {
            AgeGroup = ageGroup,
            BreedSize = breedSize,
            Ingredients = ingredients,
            NutritionalInfo = nutritionalInfo,
            StorageInstructions = storageInstructions,
            WeightKg = weightKg
        };
    }
}
