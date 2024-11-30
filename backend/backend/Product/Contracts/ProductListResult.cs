namespace backend.Product.Contracts;

public class ProductListResult<T>
{
    public List<T> Items { get; init; } = [];
    public string? NextPageToken { get; init; }
}