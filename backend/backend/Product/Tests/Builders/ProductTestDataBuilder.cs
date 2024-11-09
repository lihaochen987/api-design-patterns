using AutoFixture;
using backend.Product.DomainModels;

namespace backend.Product.Tests.Builders;

public class ProductTestDataBuilder
{
    private readonly Fixture _fixture;
    private int? _id;
    private string _name;
    private decimal _basePrice;
    private DiscountPercentage _discountPercentage;
    private decimal _taxRate;
    private Category _category;
    private Dimensions _dimensions;

    public ProductTestDataBuilder()
    {
        _fixture = new Fixture();
        _name = _fixture.Create<string>();
        _basePrice = _fixture.Create<decimal>();
        _discountPercentage = _fixture.Create<DiscountPercentage>();
        _taxRate = _fixture.Create<decimal>();
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

    public ProductTestDataBuilder WithBasePrice(decimal price)
    {
        _basePrice = price;
        return this;
    }

    public ProductTestDataBuilder WithDiscountPercentage(DiscountPercentage discountPercentage)
    {
        _discountPercentage = discountPercentage;
        return this;
    }

    public ProductTestDataBuilder WithTaxRate(decimal taxRate)
    {
        _taxRate = taxRate;
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
            _basePrice,
            _discountPercentage,
            _taxRate,
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
                .WithBasePrice(_fixture.Create<decimal>())
                .WithDiscountPercentage(_fixture.Create<DiscountPercentage>())
                .WithTaxRate(_fixture.Create<decimal>())
                .WithCategory(_fixture.Create<Category>())
                .WithDimensions(_fixture.Create<Dimensions>())
                .Build())
            .ToList();
    }
}