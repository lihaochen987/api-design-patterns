namespace backend.Product.DomainModels;

public class Product
{
    private Product()
    {
    }

    public Product(
        long id,
        string name,
        Category category,
        Dimensions dimensions
    )
    {
        Id = id;
        EnforceInvariants(
            name,
            category);
        Name = name;
        Category = category;
        Dimensions = dimensions;
    }

    public Product(
        string name,
        Category category,
        Dimensions dimensions
    )
    {
        EnforceInvariants(
            name,
            category);
        Name = name;
        Category = category;
        Dimensions = dimensions;
    }

    public long Id { get; private set; }
    public string Name { get; private set; }
    public Category Category { get; private set; }

    // public decimal Price
    // {
    //     get => CalculatePrice();
    //     init => _ = value;
    // }

    public Dimensions Dimensions { get; private set; }

    // private decimal CalculatePrice()
    // {
    //     var discountedPrice = BasePrice * (1 - (decimal)DiscountPercentage / 100);
    //     var finalPrice = discountedPrice * (1 + TaxRate / 100);
    //     return Math.Round(finalPrice, 2);
    // }

    public void Replace(
        string name,
        Category category,
        Dimensions dimensions)
    {
        EnforceInvariants(
            name,
            category);
        Name = name;
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