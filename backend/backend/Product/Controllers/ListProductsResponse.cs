namespace backend.Product.Controllers;

public class ListProductsResponse
{
    public IEnumerable<Product>? Results { get; set; }
}