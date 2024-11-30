using System.Globalization;
using backend.Product.DomainModels;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.ProductPricingControllers;

public class GetProductPricingExtensions
{
    public GetProductPricingResponse ToGetProductPricingResponse(Pricing pricing, long productId)
    {
        return new GetProductPricingResponse
        {
            Id = productId.ToString(),
            BasePrice = pricing.BasePrice.ToString(CultureInfo.InvariantCulture),
            DiscountPercentage = pricing.DiscountPercentage.ToString(),
            TaxRate = pricing.TaxRate.ToString(),
        };
    }
}