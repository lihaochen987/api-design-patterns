using backend.Product.DomainModels;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.ViewModels;

public class ProductPricingView
{
    public ProductPricingView()
    {
    }

    public long Id { get; init; }
    public Pricing Pricing { get; init; }
}