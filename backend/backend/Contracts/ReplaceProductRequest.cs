using backend.Models;

namespace backend.Contracts;

public class ReplaceProductRequest
{
    public string ProductName { get; set; } = "default";
    public string ProductPrice { get; set; } = "default";
    public string ProductCategory { get; set; } = "default";
}