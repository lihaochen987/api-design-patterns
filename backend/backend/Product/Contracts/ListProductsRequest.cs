namespace backend.Product.Contracts;

public class ListProductsRequest
{
    public string? Parent { get; set; }
    public string? Filter { get; set; }
}