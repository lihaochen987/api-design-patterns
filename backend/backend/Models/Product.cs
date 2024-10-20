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

    public int ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal ProductPrice { get; private set; }
    public ProductCategory ProductCategory { get; private set; }
}