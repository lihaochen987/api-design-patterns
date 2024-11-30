using AutoFixture;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.ViewModels;

namespace backend.Product.Tests.Builders;

public class ProductPricingTestDataBuilder
{
    private readonly Fixture _fixture;
    private int _id;
    private Pricing _pricing;

    public ProductPricingTestDataBuilder()
    {
        _fixture = new Fixture();
        _fixture.Customizations.Add(new ProductPricingBuilder());

        _id = _fixture.Create<int>();
        _pricing = _fixture.Create<Pricing>();
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
