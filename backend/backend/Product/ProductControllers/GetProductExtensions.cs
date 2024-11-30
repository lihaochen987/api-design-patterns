using System.Globalization;
using backend.Product.Contracts;
using backend.Product.ViewModels;
using backend.Shared;

namespace backend.Product.ProductControllers;

public class GetProductExtensions(TypeParser typeParser)
{
    public GetProductResponse ToGetProductResponse(ProductView product) =>
        new()
        {
            Id = product.Id.ToString(),
            Name = product.Name,
            Category = product.Category.ToString(),
            Price = product.Price.ToString(CultureInfo.InvariantCulture),
            Dimensions = new DimensionsContract
            {
                Length = product.Dimensions.Length.ToString(CultureInfo.InvariantCulture),
                Width = product.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Height = product.Dimensions.Height.ToString(CultureInfo.InvariantCulture)
            },
            AgeGroup = product.AgeGroup.HasValue ? product.AgeGroup.ToString() : null,
            BreedSize = product.BreedSize.HasValue ? product.BreedSize.ToString() : null,
            Ingredients = product.Ingredients,
            NutritionalInfo = product.NutritionalInfo is { Count: > 1 }
                ? typeParser.ParseDictionaryToString(product.NutritionalInfo, "Invalid nutritional info")
                : null,
            StorageInstructions = product.StorageInstructions,
            WeightKg = product.WeightKg.HasValue ? product.WeightKg.ToString() : null,
            IsNatural = product.IsNatural,
            IsHypoAllergenic = product.IsHypoallergenic,
            UsageInstructions = product.UsageInstructions,
            IsCrueltyFree = product.IsCrueltyFree,
            SafetyWarnings = product.SafetyWarnings
        };
}
