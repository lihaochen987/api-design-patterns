using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.ViewModels;

public class ProductPricingView
{
    public long Id { get; init; }
    public Pricing Pricing { get; init; }
}
