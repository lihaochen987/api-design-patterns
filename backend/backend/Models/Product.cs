using backend.Contracts;

namespace backend.Models;

public class Product
{
    public Product(
        long productId,
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

    public static Product MapCreateRequestToProduct(CreateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ProductName))
            throw new ArgumentException("Product name cannot be null or empty");
        if (!decimal.TryParse(request.ProductPrice, out var productPrice))
            throw new ArgumentException("Product price cannot be converted to decimal");
        if (!Enum.TryParse(request.ProductCategory, out ProductCategory productCategory))
            throw new ArgumentException("An invalid Product Category has been entered");

        return new Product(request.ProductName, productPrice, productCategory);
    }
}