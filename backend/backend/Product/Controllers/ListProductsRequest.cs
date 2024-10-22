namespace backend.Product.Controllers;

public class ListProductsRequest
{
    public string? Parent { get; set; }
    public string? Filter { get; set; }
}