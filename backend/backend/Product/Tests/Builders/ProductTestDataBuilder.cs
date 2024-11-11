using AutoFixture;
using backend.Product.DomainModels;

namespace backend.Product.Tests.Builders;

public class ProductTestDataBuilder
{
    private readonly Fixture _fixture;
    private int? _id;
    private string _name;
    private ProductPricing _pricing;
    private Category _category;
    private Dimensions _dimensions;

    public ProductTestDataBuilder()
    {
        _fixture = new Fixture();
        _fixture.Customizations.Add(new ProductPricingBuilder());

        _name = _fixture.Create<string>();
        _category = _fixture.Create<Category>();
        _dimensions = _fixture.Create<Dimensions>();
        _pricing = _fixture.Create<ProductPricing>();
    }

    public ProductTestDataBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public ProductTestDataBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductTestDataBuilder WithCategory(Category category)
    {
        _category = category;
        return this;
    }

    public ProductTestDataBuilder WithDimensions(Dimensions dimensions)
    {
        _dimensions = dimensions;
        return this;
    }

    public ProductTestDataBuilder WithPricing(ProductPricing pricing)
    {
        _pricing = pricing;
        return this;
    }

    public ProductTestDataBuilder WithPriceLessThan(decimal maxPrice)
    {
        // Hardcode discount percentage to 10 and taxRate to 5 and then calculate an appropriate BasePrice
        var basePrice = maxPrice / ((1 - (decimal)10 / 100) * (1 + (decimal)5 / 100)) - 1m;
        _pricing = new ProductPricing(basePrice, 5, 10);

        return this;
    }

    public DomainModels.Product Build()
    {
        return new DomainModels.Product(
            _id ?? _fixture.Create<int>(),
            _name,
            _pricing,
            _category,
            _dimensions);
    }

    // Todo: Indicator to change this is not apparent, might need to fix somehow.
    public List<DomainModels.Product> CreateMany(int count, int startId = 1)
    {
        return Enumerable.Range(startId, count)
            .Select(id => new ProductTestDataBuilder()
                .WithId(id)
                .WithName(_fixture.Create<string>())
                .WithPricing(_fixture.Create<ProductPricing>())
                .WithCategory(_fixture.Create<Category>())
                .WithDimensions(_fixture.Create<Dimensions>())
                .Build())
            .ToList();
    }
}