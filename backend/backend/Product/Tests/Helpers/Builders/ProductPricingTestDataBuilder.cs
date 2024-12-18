using AutoFixture;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.DomainModels.Views;

namespace backend.Product.Tests.Helpers.Builders;

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

    public ProductPricingTestDataBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public ProductPricingTestDataBuilder WithPricing(Pricing pricing)
    {
        _pricing = pricing;
        return this;
    }

    public ProductPricingView Build() => new() { Id = _id, Pricing = _pricing };
}
