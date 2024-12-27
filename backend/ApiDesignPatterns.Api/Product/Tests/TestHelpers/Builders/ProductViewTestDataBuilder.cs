using AutoFixture;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.DomainModels.Views;

namespace backend.Product.Tests.TestHelpers.Builders;

public class ProductViewTestDataBuilder
{
    private readonly Fixture _fixture;
    private readonly AgeGroup _ageGroup;
    private readonly BreedSize _breedSize;
    private Category _category;
    private Dimensions _dimensions;
    private int? _id;
    private readonly string _ingredients;
    private string _name;
    private readonly Dictionary<string, object> _nutritionalInfo;
    private decimal _price;
    private readonly string _storageInstructions;
    private readonly decimal _weightKg;

    public ProductViewTestDataBuilder()
    {
        _fixture = new Fixture();
        _fixture.Customizations.Add(new ProductPricingBuilder());
        _fixture.Customizations.Add(new ProductDimensionsBuilder());

        _name = _fixture.Create<string>();
        _category = _fixture.Create<Category>();
        _dimensions = _fixture.Create<Dimensions>();
        _price = _fixture.Create<long>();
        _ageGroup = _fixture.Create<AgeGroup>();
        _breedSize = _fixture.Create<BreedSize>();
        _ingredients = _fixture.Create<string>();
        _nutritionalInfo = _fixture.Create<Dictionary<string, object>>();
        _storageInstructions = _fixture.Create<string>();
        _weightKg = _fixture.Create<decimal>();
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

    public ProductViewTestDataBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    public ProductViewTestDataBuilder WithDimensions(Dimensions dimensions)
    {
        _dimensions = dimensions;
        return this;
    }

    public IEnumerable<ProductView> CreateMany(int count)
    {
        var products = new List<ProductView>();

        for (int i = 0; i < count; i++)
        {
            products.Add(Build());
        }

        return products;
    }

    public ProductView Build() =>
        new()
        {
            Id = _id ?? _fixture.Create<int>(),
            Name = _name,
            Price = _price,
            Category = _category,
            Dimensions = _dimensions,
            AgeGroup = _ageGroup,
            BreedSize = _breedSize,
            Ingredients = _ingredients,
            NutritionalInfo = _nutritionalInfo,
            StorageInstructions = _storageInstructions,
            WeightKg = _weightKg
        };
}
