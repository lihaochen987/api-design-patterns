using System.Globalization;
using backend.Product.DomainModels;

namespace backend.Product.ProductPricingControllers;

public class UpdateProductPricingExtensions
{
    public UpdateProductPricingResponse ToUpdateProductPricingResponse(ProductPricing productPricing, long productId)
    {
        return new UpdateProductPricingResponse
        {
            Id = productId.ToString(),
            BasePrice = productPricing.BasePrice.ToString(CultureInfo.InvariantCulture),
            DiscountPercentage = productPricing.DiscountPercentage.ToString(),
            TaxRate = productPricing.TaxRate.ToString()
        };
    }
}