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

    public int ProductId { get; init; }
    public string ProductName { get; init; }
    public decimal ProductPrice { get; init; }
    public ProductCategory ProductCategory { get; init; }
}