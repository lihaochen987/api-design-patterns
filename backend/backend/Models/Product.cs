using System.ComponentModel.DataAnnotations;
using backend.Contracts;

namespace backend.Models;

public class Product
{
    public Product(
        int productId,
        string productName,
        decimal productPrice,
        ProductCategory productCategory
    )
    {
        ProductId = productId;
        ProductName = productName;
        ProductPrice = productPrice;
        ProductCategory = productCategory;
    }

    public Product(
        string productName,
        decimal productPrice,
        ProductCategory productCategory
    )
    {
        ProductName = productName;
        ProductPrice = productPrice;
        ProductCategory = productCategory;
    }

    public long ProductId { get; private set; }

    [MaxLength(100)] private string ProductName { get; set; }
    public decimal ProductPrice { get; private set; }
    public ProductCategory ProductCategory { get; private set; }

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
        if (!Enum.TryParse(contract.ProductCategory, out ProductCategory productCategory))
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
        ProductName = product.ProductName;
        ProductPrice = product.ProductPrice;
        ProductCategory = product.ProductCategory;
    }
}