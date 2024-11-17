using backend.Product.DomainModels;

namespace backend.Product.ViewModels;

public class ProductPricingView
{
    public long Id { get; set; }
    public Pricing Pricing { get; set; }
}