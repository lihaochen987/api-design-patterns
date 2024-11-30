using System.Globalization;
using backend.Product.Contracts;
using backend.Product.DomainModels;
using backend.Shared;

namespace backend.Product.ProductControllers;

public class UpdateProductExtensions(TypeParser typeParser)
{
    public UpdateProductResponse ToUpdateProductResponse(DomainModels.Product product)
    {
        var response = new UpdateProductResponse
        {
            Id = product.Id.ToString(),
            Name = product.Name,
            Category = product.Category.ToString(),
            Pricing = new ProductPricingContract
            {
                BasePrice = product.Pricing.BasePrice.ToString(CultureInfo.InvariantCulture),
                DiscountPercentage = product.Pricing.DiscountPercentage.ToString(CultureInfo.InvariantCulture),
                TaxRate = product.Pricing.TaxRate.ToString(CultureInfo.InvariantCulture)
            },
            Dimensions = new DimensionsContract
            {
                Length = product.Dimensions.Length.ToString(CultureInfo.InvariantCulture),
                Width = product.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Height = product.Dimensions.Height.ToString(CultureInfo.InvariantCulture)
            }
        };

        if (product is PetFood petFood)
        {
            response.AgeGroup = petFood.AgeGroup.ToString();
            response.BreedSize = petFood.BreedSize.ToString();
            response.Ingredients = petFood.Ingredients;
            response.NutritionalInfo =
                typeParser.ParseDictionaryToString(petFood.NutritionalInfo, "Invalid nutritional info");
            response.StorageInstructions = petFood.StorageInstructions;
            response.WeightKg = petFood.WeightKg.ToString(CultureInfo.InvariantCulture);
        }

        if (product is GroomingAndHygiene groomingAndHygiene)
        {
            response.IsNatural = groomingAndHygiene.IsNatural;
            response.IsHypoAllergenic = groomingAndHygiene.IsHypoallergenic;
            response.UsageInstructions = groomingAndHygiene.UsageInstructions;
            response.IsCrueltyFree = groomingAndHygiene.IsCrueltyFree;
            response.SafetyWarnings = groomingAndHygiene.SafetyWarnings;
        }

        return response;
    }
}