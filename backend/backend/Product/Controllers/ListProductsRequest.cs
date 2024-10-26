namespace backend.Product.Controllers;

public class ListProductsRequest
{
    public string? Parent { get; set; }
    public string? Filter { get; set; }
    public string? PageToken { get; set; } = "";
    public int MaxPageSize { get; set; } = 10;
}