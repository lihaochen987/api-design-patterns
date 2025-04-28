using AutoFixture;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.Tests.TestHelpers.SpecimenBuilders;

namespace backend.Product.Tests.TestHelpers.Builders;

public class ProductTestDataBuilder
{
    private readonly AgeGroup _ageGroup;
    private readonly BreedSize _breedSize;
    private Category _category;
    private Dimensions _dimensions;
    private long _id;
    private readonly string _ingredients;
    private Name _name;
    private readonly Dictionary<string, object> _nutritionalInfo;
    private Pricing _pricing;
    private readonly string _storageInstructions;
    private readonly decimal _weightKg;
    private readonly bool _isNatural;
    private readonly bool _isHypoAllergenic;
    private readonly string _usageInstructions;
    private readonly bool _isCrueltyFree;
    private readonly string _safetyWarnings;

    public ProductTestDataBuilder()
    {
        Fixture fixture = new();
        fixture.Customizations.Add(new ProductPricingSpecimenBuilder());
        fixture.Customizations.Add(new DimensionsSpecimenBuilder());
        fixture.Customizations.Add(new NameSpecimenBuilder());

        _id = fixture.Create<long>();
        _name = fixture.Create<Name>();
        _category = fixture.Create<Category>();
        _dimensions = fixture.Create<Dimensions>();
        _pricing = fixture.Create<Pricing>();

        _ageGroup = fixture.Create<AgeGroup>();
        _breedSize = fixture.Create<BreedSize>();
        _ingredients = fixture.Create<string>();
        _nutritionalInfo = fixture.Create<Dictionary<string, object>>();
        _storageInstructions = fixture.Create<string>();
        _weightKg = fixture.Create<decimal>();

        _isNatural = fixture.Create<bool>();
        _isHypoAllergenic = fixture.Create<bool>();
        _usageInstructions = fixture.Create<string>();
        _isCrueltyFree = fixture.Create<bool>();
        _safetyWarnings = fixture.Create<string>();
    }

    public ProductTestDataBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public ProductTestDataBuilder WithName(Name name)
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

    // Todo: Refactor this
    public DomainModels.Product Build()
    {
        return _category switch
        {
            Category.PetFood => new PetFood
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
            },
            Category.GroomingAndHygiene => new GroomingAndHygiene
            {
                Id = _id,
                Name = _name,
                Category = _category,
                Pricing = _pricing,
                Dimensions = _dimensions,
                IsNatural = _isNatural,
                IsHypoallergenic = _isHypoAllergenic,
                UsageInstructions = _usageInstructions,
                IsCrueltyFree = _isCrueltyFree,
                SafetyWarnings = _safetyWarnings
            },
            _ => new DomainModels.Product
            {
                Id = _id,
                Name = _name,
                Category = _category,
                Pricing = _pricing,
                Dimensions = _dimensions,
            }
        };
    }
}
