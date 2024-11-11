using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Product.DomainModels;

public class Product
{
    private Product()
    {
    }

    public Product(
        long id,
        string name,
        ProductPricing pricing,
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

    public Product(
        string name,
        ProductPricing pricing,
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

    public long Id { get; private set; }
    public string Name { get; private set; }
    public Category Category { get; private set; }
    public ProductPricing Pricing { get; private set; }

    public decimal Price
    {
        get => CalculatePrice();
        init => _ = value;
    }

    public Dimensions Dimensions { get; private set; }

    private decimal CalculatePrice()
    {
        var discountedPrice = Pricing.BasePrice * (1 - (decimal)Pricing.DiscountPercentage / 100);
        var finalPrice = discountedPrice * (1 + Pricing.TaxRate / 100);
        return Math.Round(finalPrice, 2);
    }

    public void Replace(
        string name,
        ProductPricing pricing,
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

    private static void EnforceInvariants(
        string name,
        Category category)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name is required.");
        if (!Enum.IsDefined(typeof(Category), category))
            throw new ArgumentException("Invalid category for the Product.");
    }
}