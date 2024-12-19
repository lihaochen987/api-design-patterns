namespace backend.Product.ProductControllers;

public record ListProductsResponse
{
    public IEnumerable<object?> Results { get; init; } = [];
    public string? NextPageToken { get; init; }
}
