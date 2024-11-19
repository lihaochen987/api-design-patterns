using AutoFixture;
using backend.Product.DomainModels;
using backend.Product.ViewModels;

namespace backend.Product.Tests.Builders;

public class ProductViewTestDataBuilder
{
    private readonly Fixture _fixture;
    private int? _id;
    private string _name;
    private decimal _price;
    private Category _category;
    private Dimensions _dimensions;

    public ProductViewTestDataBuilder()
    {
        _fixture = new Fixture();
        _fixture.Customizations.Add(new ProductPricingBuilder());
        _fixture.Customizations.Add(new ProductDimensionsBuilder());

        _name = _fixture.Create<string>();
        _category = _fixture.Create<Category>();
        _dimensions = _fixture.Create<Dimensions>();
        _price = _fixture.Create<long>();
    }

    public ProductViewTestDataBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public ProductViewTestDataBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductViewTestDataBuilder WithCategory(Category category)
    {
        _category = category;
        return this;
    }

    public ProductViewTestDataBuilder WithDimensions(Dimensions dimensions)
    {
        _dimensions = dimensions;
        return this;
    }

    public ProductView Build()
    {
        return new ProductView(
            _id ?? _fixture.Create<int>(),
            _name,
            _price,
            _category,
            _dimensions
        );
    }
}