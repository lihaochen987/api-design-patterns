using backend.Product.DomainModels.ValueObjects;
using backend.Shared;

namespace backend.Product.DomainModels.Views;

public class ProductPricingView : Entity
{
    public required Pricing Pricing { get; init; }
}
