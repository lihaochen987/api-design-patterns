using System.ComponentModel.DataAnnotations;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Shared;

namespace backend.Product.DomainModels;

public abstract class Product : Entity
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Product()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    protected Product(
        long id,
        string name,
        Pricing pricing,
        Category category,
        Dimensions dimensions
    )
    {
        Id = id;
        EnforceInvariants(
            name,
            category);
        Name = name;
        Pricing = pricing;
        Category = category;
        Dimensions = dimensions;
    }

    protected Product(
        string name,
        Pricing pricing,
        Category category,
        Dimensions dimensions
    )
    {
        EnforceInvariants(
            name,
            category);
        Name = name;
        Pricing = pricing;
        Category = category;
        Dimensions = dimensions;
    }

    [MaxLength(100)] public string Name { get; private set; }

    public Category Category { get; private set; }

    public Pricing Pricing { get; private set; }

    public Dimensions Dimensions { get; private set; }

    public void Replace(
        string name,
        Pricing pricing,
        Category category,
        Dimensions dimensions)
    {
        EnforceInvariants(
            name,
            category);
        Name = name;
        Pricing = pricing;
        Category = category;
        Dimensions = dimensions;
    }

    public void UpdatePricing(
        decimal basePrice,
        decimal discountPercentage,
        decimal taxRate) =>
        Pricing = new Pricing(basePrice, discountPercentage, taxRate);

    private static void EnforceInvariants(
        string name,
        Category category)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name is required.");
        }

        if (!Enum.IsDefined(typeof(Category), category))
        {
            throw new ArgumentException("Invalid category for the Product.");
        }
    }
}
