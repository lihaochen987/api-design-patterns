using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Product;

public class Product
{
    public Product(
        int id,
        string name,
        decimal price,
        Category category
    )
    {
        Id = id;
        Name = name;
        Price = price;
        Category = category;
    }

    public Product(
        string name,
        decimal price,
        Category category
    )
    {
        Name = name;
        Price = price;
        Category = category;
    }

    [Column("ProductId")]
    public long Id { get; private set; }

    [Column("ProductName")]
    [MaxLength(100)] public string Name { get; private set; }
    [Column("ProductPrice")]
    public decimal Price { get; private set; }
    [Column("ProductCategory")]
    public Category Category { get; private set; }
    
    public void Replace(
        string name, 
        decimal price, 
        Category category)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Product name is required.");
        if (price <= 0) throw new ArgumentException("Product price must be greater than zero.");
        if (!Enum.IsDefined(typeof(Category), category)) throw new ArgumentException("Invalid category.");

        Name = name;
        Price = price;
        Category = category;
    }
}