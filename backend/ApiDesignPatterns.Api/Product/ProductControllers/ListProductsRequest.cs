namespace backend.Product.ProductControllers;

public class ListProductsRequest
{
    public string? Filter { get; set; }
    public string? PageToken { get; set; } = "";
    public int MaxPageSize { get; set; } = 10;
}
