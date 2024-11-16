namespace backend.Product.DomainModels;

public class Product
{
    private Product()
    {
    }

    public Product(
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

    public Product(
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

    public long Id { get; private set; }
    public string Name { get; private set; }
    public Category Category { get; private set; }
    public Pricing Pricing { get; private set; }

    public decimal Price { get; private set; }

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
        decimal taxRate)
    {
        Pricing = new Pricing(basePrice, discountPercentage, taxRate);
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