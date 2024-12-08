using System.ComponentModel.DataAnnotations;
using backend.Product.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[SwaggerDiscriminator("category")]
[SwaggerSubType(typeof(UpdatePetFoodResponse))]
[SwaggerSubType(typeof(UpdateGroomingAndHygieneResponse))]
public class UpdateProductResponse
{
    [Required] public string Id { get; set; } = "";
    [Required] public string Name { get; set; } = "";

    [Required]
    public ProductPricingResponse Pricing { get; set; } =
        new() { BasePrice = "", DiscountPercentage = "", TaxRate = "" };

    [Required] public string Category { get; set; } = "";
    [Required] public DimensionsResponse Dimensions { get; set; } = new() { Length = "", Width = "", Height = "" };
}

public class UpdateGroomingAndHygieneResponse : UpdateProductResponse
{
    [Required] public bool IsNatural { get; init; }
    [Required] public bool IsHypoAllergenic { get; init; }
    [Required] public string UsageInstructions { get; init; } = "";
    [Required] public bool IsCrueltyFree { get; init; }
    [Required] public string SafetyWarnings { get; init; } = "";
}

public class UpdatePetFoodResponse : UpdateProductResponse
{
    [Required] public string AgeGroup { get; init; } = "";
    [Required] public string BreedSize { get; init; } = "";
    [Required] public string Ingredients { get; init; } = "";
    [Required] public string NutritionalInfo { get; init; } = "";
    [Required] public string StorageInstructions { get; init; } = "";
    [Required] public string WeightKg { get; init; } = "";
}
