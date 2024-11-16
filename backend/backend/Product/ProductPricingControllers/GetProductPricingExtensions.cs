using System.Globalization;
using backend.Product.DomainModels;

namespace backend.Product.ProductPricingControllers;

public class GetProductPricingExtensions
{
    public GetProductPricingResponse ToGetProductPricingResponse(ProductPricing productPricing, long productId)
    {
        return new GetProductPricingResponse
        {
            Id = productId.ToString(),
            BasePrice = productPricing.BasePrice.ToString(CultureInfo.InvariantCulture),
            DiscountPercentage = productPricing.DiscountPercentage.ToString(),
            TaxRate = productPricing.TaxRate.ToString(),
        };
    }
}