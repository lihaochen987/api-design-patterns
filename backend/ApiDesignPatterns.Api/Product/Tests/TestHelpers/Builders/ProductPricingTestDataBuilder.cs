using AutoFixture;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.DomainModels.Views;

namespace backend.Product.Tests.TestHelpers.Builders;

public class ProductPricingTestDataBuilder
{
    private int _id;
    private Pricing _pricing;

    public ProductPricingTestDataBuilder()
    {
        Fixture fixture = new();
        fixture.Customizations.Add(new ProductPricingBuilder());

        _id = fixture.Create<int>();
        _pricing = fixture.Create<Pricing>();
    }

    public ProductPricingView Build() => new() { Id = _id, Pricing = _pricing };
}
