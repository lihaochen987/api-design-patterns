using System.Globalization;
using backend.Product.DomainModels;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.ProductPricingControllers;

public class UpdateProductPricingExtensions
{
    public UpdateProductPricingResponse ToUpdateProductPricingResponse(Pricing pricing, long productId)
    {
        return new UpdateProductPricingResponse
        {
            Id = productId.ToString(),
            BasePrice = pricing.BasePrice.ToString(CultureInfo.InvariantCulture),
            DiscountPercentage = pricing.DiscountPercentage.ToString(CultureInfo.InvariantCulture),
            TaxRate = pricing.TaxRate.ToString(CultureInfo.InvariantCulture)
        };
    }
}