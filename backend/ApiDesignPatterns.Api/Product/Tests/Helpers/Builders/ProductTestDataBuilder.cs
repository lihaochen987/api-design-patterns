using AutoFixture;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.Tests.Helpers.Builders;

public class ProductTestDataBuilder
{
    private readonly AgeGroup _ageGroup;
    private readonly BreedSize _breedSize;
    private Category _category;
    private Dimensions _dimensions;
    private long _id;
    private readonly string _ingredients;
    private string _name;
    private readonly Dictionary<string, object> _nutritionalInfo;
    private Pricing _pricing;
    private readonly string _storageInstructions;
    private readonly decimal _weightKg;

    public ProductTestDataBuilder()
    {
        Fixture fixture = new();
        fixture.Customizations.Add(new ProductPricingBuilder());
        fixture.Customizations.Add(new ProductDimensionsBuilder());

        _id = fixture.Create<long>();
        _name = fixture.Create<string>();
        _category = fixture.Create<Category>();
        _dimensions = fixture.Create<Dimensions>();
        _pricing = fixture.Create<Pricing>();

        _ageGroup = fixture.Create<AgeGroup>();
        _breedSize = fixture.Create<BreedSize>();
        _ingredients = fixture.Create<string>();
        _nutritionalInfo = fixture.Create<Dictionary<string, object>>();
        _storageInstructions = fixture.Create<string>();
        _weightKg = fixture.Create<decimal>();
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
        decimal basePrice = maxPrice / ((1 - (decimal)10 / 100) * (1 + (decimal)5 / 100)) - 2m;
        _pricing = new Pricing { BasePrice = basePrice, DiscountPercentage = 5, TaxRate = 10 };

        return this;
    }

    public DomainModels.Product Build()
    {
        if (_category == Category.PetFood)
        {
            return new PetFood
            {
                Id = _id,
                Name = _name,
                Category = _category,
                Pricing = _pricing,
                Dimensions = _dimensions,
                AgeGroup = _ageGroup,
                BreedSize = _breedSize,
                Ingredients = _ingredients,
                NutritionalInfo = _nutritionalInfo,
                StorageInstructions = _storageInstructions,
                WeightKg = _weightKg
            };
        }

        return new DomainModels.Product
        {
            Id = _id,
            Name = _name,
            Category = _category,
            Pricing = _pricing,
            Dimensions = _dimensions,
        };
    }
}
