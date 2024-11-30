using AutoFixture;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.Tests.Builders;

public class ProductTestDataBuilder
{
    private readonly Fixture _fixture;
    private int? _id;
    private string _name;
    private Pricing _pricing;
    private Category _category;
    private Dimensions _dimensions;
    private AgeGroup _ageGroup;
    private BreedSize _breedSize;
    private string _ingredients;
    private Dictionary<string, object> _nutritionalInfo;
    private string _storageInstructions;
    private decimal _weightKg;

    public ProductTestDataBuilder()
    {
        _fixture = new Fixture();
        _fixture.Customizations.Add(new ProductPricingBuilder());
        _fixture.Customizations.Add(new ProductDimensionsBuilder());

        _name = _fixture.Create<string>();
        _category = _fixture.Create<Category>();
        _dimensions = _fixture.Create<Dimensions>();
        _pricing = _fixture.Create<Pricing>();

        _ageGroup = _fixture.Create<AgeGroup>();
        _breedSize = _fixture.Create<BreedSize>();
        _ingredients = _fixture.Create<string>();
        _nutritionalInfo = _fixture.Create<Dictionary<string, object>>();
        _storageInstructions = _fixture.Create<string>();
        _weightKg = _fixture.Create<decimal>();
    }

    public ProductTestDataBuilder AsToys()
    {
        _category = Category.Toys;
        return this;
    }

    public ProductTestDataBuilder AsPetFood()
    {
        _category = Category.PetFood;
        return this;
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

    public ProductTestDataBuilder WithPricing(Pricing pricing)
    {
        _pricing = pricing;
        return this;
    }

    public ProductTestDataBuilder WithPriceLessThan(decimal maxPrice)
    {
        // Hardcode discount percentage to 10 and taxRate to 5 and then calculate an appropriate BasePrice
        var basePrice = maxPrice / ((1 - (decimal)10 / 100) * (1 + (decimal)5 / 100)) - 2m;
        _pricing = new Pricing(basePrice, 5, 10);

        return this;
    }

    public DomainModels.Product Build()
    {
        if (_category == Category.PetFood)
        {
            return new PetFood(
                _name,
                _pricing,
                _dimensions,
                _ageGroup,
                _breedSize,
                _ingredients,
                _nutritionalInfo,
                _storageInstructions,
                _weightKg);
        }

        return new BaseProduct(
            _id ?? _fixture.Create<int>(),
            _name,
            _pricing,
            _category,
            _dimensions);
    }
}