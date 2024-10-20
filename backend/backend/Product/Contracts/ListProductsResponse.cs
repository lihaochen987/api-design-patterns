namespace backend.Product.Contracts;

public class ListProductsResponse
{
    public IEnumerable<Product>? Results { get; set; }
}