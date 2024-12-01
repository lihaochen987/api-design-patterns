using System.ComponentModel.DataAnnotations;
using backend.Product.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[SwaggerDiscriminator("category")]
[SwaggerSubType(typeof(CreatePetFoodResponse))]
[SwaggerSubType(typeof(CreateGroomingAndHygieneResponse))]
public class CreateProductResponse
{
    [Required] public string Name { get; set; } = "";

    [Required]
    public ProductPricingContract Pricing { get; set; } =
        new() { BasePrice = "", DiscountPercentage = "", TaxRate = "" };

    [Required] public string Category { get; set; } = "";

    [Required] public DimensionsContract Dimensions { get; set; } = new() { Length = "", Width = "", Height = "" };
}

public class CreateGroomingAndHygieneResponse : CreateProductResponse
{
    [Required] public bool IsNatural { get; init; }
    [Required] public bool IsHypoAllergenic { get; init; }
    [Required] public string UsageInstructions { get; init; } = "";
    [Required] public bool IsCrueltyFree { get; init; }
    [Required] public string SafetyWarnings { get; init; } = "";
}

public class CreatePetFoodResponse : CreateProductResponse
{
    [Required] public string AgeGroup { get; init; } = "";
    [Required] public string BreedSize { get; init; } = "";
    [Required] public string Ingredients { get; init; } = "";
    [Required] public string NutritionalInfo { get; init; } = "";
    [Required] public string StorageInstructions { get; init; } = "";
    [Required] public string WeightKg { get; init; } = "";
}
