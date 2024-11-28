using backend.Product.DomainModels;

namespace backend.Product.ViewModels;

public class ProductPricingView
{
    public ProductPricingView()
    {
    }

    public ProductPricingView(long id, Pricing pricing)
    {
        Id = id;
        Pricing = pricing;
    }

    public long Id { get; init; }
    public Pricing Pricing { get; init; }
}