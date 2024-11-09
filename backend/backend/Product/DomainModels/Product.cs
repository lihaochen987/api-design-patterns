using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Product.DomainModels;

[Table("products")]
public class Product
{
    // ReSharper disable once UnusedMember.Local - [Justification]:Empty constructor is being used to keep EFCore happy
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Product()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
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

    [Column("product_id")] public long Id { get; private set; }

    [Column("product_name")]
    [MaxLength(100)]
    public string Name { get; private set; }

    [Column("product_price")] public decimal Price { get; private set; }
    [Column("product_category")] public Category Category { get; private set; }
    public Dimensions Dimensions { get; private set; }

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