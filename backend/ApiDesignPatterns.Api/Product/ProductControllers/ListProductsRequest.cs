namespace backend.Product.ProductControllers;

public record ListProductsRequest
{
    public string? Filter { get; init; }
    public string? PageToken { get; init; } = "";
    public int MaxPageSize { get; init; } = 10;
}
