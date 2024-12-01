using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.DomainModels.Views;

public class ProductPricingView
{
    public long Id { get; init; }
    public Pricing Pricing { get; init; }
}
