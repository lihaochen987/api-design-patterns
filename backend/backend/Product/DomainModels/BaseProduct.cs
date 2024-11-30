using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.DomainModels;

public class BaseProduct : Product
{
    private BaseProduct()
    {
    }

    public BaseProduct(
        long id,
        string name,
        Pricing pricing,
        Category category,
        Dimensions dimensions)
        : base(id, name, pricing, category, dimensions)
    {
    }

    public BaseProduct(
        string name,
        Pricing pricing,
        Category category,
        Dimensions dimensions)
        : base(name, pricing, category, dimensions)
    {
    }
}
