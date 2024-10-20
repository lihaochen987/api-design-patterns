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
    public string ProductName { get; private set; }
    public decimal ProductPrice { get; private set; }
    public ProductCategory ProductCategory { get; private set; }

    /// <summary>
    /// I quite like having this TryParse method which maps and chains error messages to an actual product
    /// </summary>
    public static bool TryParse(
        ProductContract contract,
        out Product? product,
        out IList<string> errorMessages)
    {
        product = null;
        errorMessages = [];

        if (string.IsNullOrWhiteSpace(contract.ProductName)) errorMessages.Add("Product name is required.");
        if (!decimal.TryParse(contract.ProductPrice, out var productPrice))
            errorMessages.Add("Product price is invalid.");
        if (!Enum.TryParse(contract.ProductCategory, out ProductCategory productCategory))
            errorMessages.Add("Product category is invalid.");
        if (errorMessages.Count != 0) return false;

        product = new Product(contract.ProductName, productPrice, productCategory);
        return true;
    }

    public void Replace(
        ProductContract contract,
        out IList<string> errorMessages)
    {
        if (!TryParse(contract, out var product, out errorMessages) || product == null)
        {
            return;
        }
        
        ProductName = product.ProductName;
        ProductPrice = product.ProductPrice;
        ProductCategory = product.ProductCategory;
    }
}