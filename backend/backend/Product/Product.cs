using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Product.Contracts;

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

    /// <summary>
    /// I quite like having this TryParse method which maps and chains error messages to an actual product
    /// </summary>
    public static bool TryParse(
        ProductContract contract,
        out Product? product,
        out IDictionary<string, string> errorMessages)
    {
        product = null;
        errorMessages = new Dictionary<string, string>();

        if (string.IsNullOrWhiteSpace(contract.ProductName))
            errorMessages.Add("Product.ProductName", $"Product name is required. With value '{contract.ProductName}'.");
        if (!decimal.TryParse(contract.ProductPrice, out var productPrice))
            errorMessages.Add("Product.ProductPrice", $"Product price is invalid. With value {contract.ProductPrice}.");
        if (!Enum.TryParse(contract.ProductCategory, out Category productCategory))
            errorMessages.Add("Product.ProductCategory",
                $"Product category is invalid with value {contract.ProductCategory}");
        if (errorMessages.Count != 0) return false;

        product = new Product(contract.ProductName, productPrice, productCategory);
        return true;
    }

    public void UpdateProductDetails(
        ProductContract contract,
        out IDictionary<string, string> errorMessages)
    {
        if (!TryParse(contract, out var product, out errorMessages) || product == null) return;
        Name = product.Name;
        Price = product.Price;
        Category = product.Category;
    }
}