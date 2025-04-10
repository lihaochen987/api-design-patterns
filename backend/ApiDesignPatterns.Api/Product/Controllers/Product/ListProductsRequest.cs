namespace backend.Product.Controllers.Product;

public record ListProductsRequest
{
    public string? Filter { get; init; }
    public string? PageToken { get; init; } = "";
    public int MaxPageSize { get; init; } = 10;
}
