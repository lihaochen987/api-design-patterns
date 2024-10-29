using AutoFixture;
using backend.Product.DomainModels;

namespace backend.Product.Tests;

public class ProductTestDataBuilder
{
    private readonly Fixture _fixture;
    private int? _id;
    private string _name;
    private decimal _price;
    private Category _category;
    private Dimensions _dimensions;

    public ProductTestDataBuilder()
    {
        _fixture = new Fixture();
        _name = _fixture.Create<string>();
        _price = _fixture.Create<decimal>();
        _category = _fixture.Create<Category>();
        _dimensions = _fixture.Create<Dimensions>();
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

    public ProductTestDataBuilder WithPrice(decimal price)
    {
        _price = price;
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

    public DomainModels.Product Build()
    {
        return new DomainModels.Product(
            _id ?? _fixture.Create<int>(),
            _name,
            _price,
            _category,
            _dimensions);
    }

    public List<DomainModels.Product> CreateMany(int count, int startId = 1)
    {
        return Enumerable.Range(startId, count)
            .Select(id => new ProductTestDataBuilder()
                .WithId(id)
                .WithName(_fixture.Create<string>())
                .WithPrice(_fixture.Create<decimal>())
                .WithCategory(_fixture.Create<Category>())
                .WithDimensions(_fixture.Create<Dimensions>())
                .Build())
            .ToList();
    }
}
