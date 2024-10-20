using backend.Models;

namespace backend.Contracts;

public class ReplaceProductRequest
{
    public string ProductName { get; set; } = "";
    public string ProductPrice { get; set; } = "";
    public string ProductCategory { get; set; } = "";
}