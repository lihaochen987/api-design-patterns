using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Product.DomainModels;

[Table("products")]
public class Product
{
    private Product()
    {
    }

    public Product(
        int id,
        string name,
        decimal basePrice,
        DiscountPercentage discountPercentage,
        decimal taxRate,
        Category category,
        Dimensions dimensions
    )
    {
        Id = id;
        EnforceInvariants(
            name,
            basePrice,
            taxRate,
            category,
            dimensions);
        Name = name;
        BasePrice = basePrice;
        DiscountPercentage = discountPercentage;
        TaxRate = taxRate;
        Category = category;
        Dimensions = dimensions;
    }

    public Product(
        string name,
        decimal basePrice,
        DiscountPercentage discountPercentage,
        decimal taxRate,
        Category category,
        Dimensions dimensions
    )
    {
        EnforceInvariants(
            name,
            basePrice,
            taxRate,
            category,
            dimensions);
        Name = name;
        BasePrice = basePrice;
        DiscountPercentage = discountPercentage;
        TaxRate = taxRate;
        Category = category;
        Dimensions = dimensions;
    }

    [Column("product_id")] public long Id { get; private set; }

    [Column("product_name")]
    [MaxLength(100)]
    public string Name { get; private set; }

    [Column("product_base_price")] public decimal BasePrice { get; private set; }

    [Column("product_discount_percentage")]
    public DiscountPercentage DiscountPercentage { get; private set; }

    [Column("product_tax_rate")] public decimal TaxRate { get; private set; }

    [Column("product_category")] public Category Category { get; private set; }

    [NotMapped] public decimal Price => CalculatePrice();

    public Dimensions Dimensions { get; private set; }

    private decimal CalculatePrice()
    {
        var discountedPrice = BasePrice * (1 - (decimal)DiscountPercentage / 100);
        var finalPrice = discountedPrice * (1 + TaxRate / 100);
        return finalPrice;
    }

    public void Replace(
        string name,
        decimal basePrice,
        DiscountPercentage discountPercentage,
        decimal taxRate,
        Category category,
        Dimensions dimensions)
    {
        EnforceInvariants(
            name,
            basePrice,
            taxRate,
            category,
            dimensions);
        Name = name;
        BasePrice = basePrice;
        DiscountPercentage = discountPercentage;
        TaxRate = taxRate;
        Category = category;
        Dimensions = dimensions;
    }

    private static void EnforceInvariants(
        string name,
        decimal basePrice,
        decimal taxRate,
        Category category,
        Dimensions dimensions)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name is required.");
        if (basePrice <= 0)
            throw new ArgumentException("Product price must be greater than zero.");
        if (taxRate < 0 || taxRate > 100)
            throw new ArgumentException("Product tax rate must be between 0 and 100%");
        if (!Enum.IsDefined(typeof(Category), category))
            throw new ArgumentException("Invalid category for the Product.");
        if (dimensions == null)
            throw new ArgumentException("Dimensions are required.");
    }
}