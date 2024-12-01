using System.ComponentModel.DataAnnotations;
using backend.Product.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[SwaggerDiscriminator("category")]
[SwaggerSubType(typeof(GetPetFoodResponse))]
[SwaggerSubType(typeof(GetGroomingAndHygieneResponse))]
public class GetProductResponse
{
    [Required] public string Id { get; init; } = "";
    [Required] public string Name { get; init; } = "";
    [Required] public string Price { get; init; } = "";
    [Required] public string Category { get; init; } = "";
    [Required] public DimensionsContract Dimensions { get; init; } = new() { Length = "", Width = "", Height = "" };
}

public class GetGroomingAndHygieneResponse : GetProductResponse
{
    [Required] public bool IsNatural { get; init; }
    [Required] public bool IsHypoAllergenic { get; init; }
    [Required] public string UsageInstructions { get; init; } = "";
    [Required] public bool IsCrueltyFree { get; init; }
    [Required] public string SafetyWarnings { get; init; } = "";
}

public class GetPetFoodResponse : GetProductResponse
{
    [Required] public string AgeGroup { get; init; } = "";
    [Required] public string BreedSize { get; init; } = "";
    [Required] public string Ingredients { get; init; } = "";
    [Required] public string NutritionalInfo { get; init; } = "";
    [Required] public string StorageInstructions { get; init; } = "";
    [Required] public string WeightKg { get; init; } = "";
}
