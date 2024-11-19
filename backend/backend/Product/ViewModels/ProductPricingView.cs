using backend.Product.DomainModels;

namespace backend.Product.ViewModels;

public class ProductPricingView(long id, Pricing pricing)
{
    public long Id { get; init; } = id;
    public Pricing Pricing { get; init; } = pricing;
}