using System.Globalization;
using backend.Product.Contracts;

namespace backend.Product.ProductControllers;

public class UpdateProductExtensions
{
    public UpdateProductResponse ToUpdateProductResponse(DomainModels.BaseProduct baseProduct)
    {
        return new UpdateProductResponse
        {
            Id = baseProduct.Id.ToString(),
            Name = baseProduct.Name,
            Category = baseProduct.Category.ToString(),
            Pricing = new ProductPricingContract
            {
                BasePrice = baseProduct.Pricing.BasePrice.ToString(CultureInfo.InvariantCulture),
                DiscountPercentage = baseProduct.Pricing.DiscountPercentage.ToString(),
                TaxRate = baseProduct.Pricing.TaxRate.ToString()
            },
            Dimensions = new DimensionsContract
            {
                Length = baseProduct.Dimensions.Length.ToString(CultureInfo.InvariantCulture),
                Width = baseProduct.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Height = baseProduct.Dimensions.Height.ToString(CultureInfo.InvariantCulture)
            }
        };
    }
}