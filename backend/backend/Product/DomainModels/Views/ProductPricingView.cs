using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.DomainModels.Views;

public class ProductPricingView
{
    public required long Id { get; init; }
    public required Pricing Pricing { get; init; }
}
