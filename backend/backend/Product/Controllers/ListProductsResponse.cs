namespace backend.Product.Controllers;

public class ListProductsResponse
{
    public IEnumerable<GetProductResponse?> Results { get; set; } = [];
    public string? NextPageToken { get; set; }
}