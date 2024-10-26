namespace backend.Product.Controllers;

public class ListProductsResponse
{
    public IEnumerable<DomainModels.Product?> Results { get; set; } = [];
    public string? NextPageToken { get; set; }
}