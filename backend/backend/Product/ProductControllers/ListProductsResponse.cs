using System.ComponentModel.DataAnnotations;

namespace backend.Product.ProductControllers;

public class ListProductsResponse
{
    [Required] public IEnumerable<object?> Results { get; set; } = [];
    [Required] public string? NextPageToken { get; set; }
}
