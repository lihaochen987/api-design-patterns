using backend.Models;

namespace backend.Contracts;

public class ListProductsResponse
{
    public IEnumerable<Product>? Results { get; set; }
}