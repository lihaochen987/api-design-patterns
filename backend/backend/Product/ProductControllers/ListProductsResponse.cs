namespace backend.Product.ProductControllers;

public class ListProductsResponse
{
    public IEnumerable<GetProductResponse?> Results { get; set; } = [];
    public string? NextPageToken { get; set; }
}
