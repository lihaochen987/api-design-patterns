using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Product.DomainModels;

public class Product : IEntityObject
{
    private Product()
    {
    }

    public Product(
        int id,
        string name,
        decimal price,
        Category category,
        Dimensions dimensions
    )
    {
        Id = id;
        EnforceInvariants(name, price, category, dimensions);
        Name = name;
        Price = price;
        Category = category;
        Dimensions = dimensions;
    }

    public Product(
        string name,
        decimal price,
        Category category,
        Dimensions dimensions
    )
    {
        EnforceInvariants(name, price, category, dimensions);
        Name = name;
        Price = price;
        Category = category;
        Dimensions = dimensions;
    }

    [Column("ProductId")] public long Id { get; private set; }

    [Column("ProductName")]
    [MaxLength(100)]
    public string Name { get; private set; }

    [Column("ProductPrice")] public decimal Price { get; private set; }
    [Column("ProductCategory")] public Category Category { get; private set; }
    [Column("ProductDimensions")] public Dimensions Dimensions { get; private set; }

    public void Replace(
        string name,
        decimal price,
        Category category,
        Dimensions dimensions)
    {
        EnforceInvariants(name, price, category, dimensions);
        Name = name;
        Price = price;
        Category = category;
        Dimensions = dimensions;
    }

    private static void EnforceInvariants(string name, decimal price, Category category, Dimensions dimensions)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name is required.");
        if (price <= 0)
            throw new ArgumentException("Product price must be greater than zero.");
        if (!Enum.IsDefined(typeof(Category), category))
            throw new ArgumentException("Invalid category for the Product.");
        if (dimensions == null)
            throw new ArgumentException("Dimensions are required.");
    }
}