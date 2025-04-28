using AutoFixture;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.DomainModels.Views;
using backend.Product.Tests.TestHelpers.SpecimenBuilders;

namespace backend.Product.Tests.TestHelpers.Builders;

public class ProductPricingViewTestDataBuilder
{
    private readonly int _id;
    private readonly Pricing _pricing;

    public ProductPricingViewTestDataBuilder()
    {
        Fixture fixture = new();
        fixture.Customizations.Add(new ProductPricingSpecimenBuilder());

        _id = fixture.Create<int>();
        _pricing = fixture.Create<Pricing>();
    }

    public ProductPricingView Build() => new() { Id = _id, Pricing = _pricing };
}
