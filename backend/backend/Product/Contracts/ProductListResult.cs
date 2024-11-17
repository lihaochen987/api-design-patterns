namespace backend.Product.Contracts;

public class ProductListResult<T>
{
    public List<T> Items { get; set; } = [];
    public string? NextPageToken { get; set; }
}