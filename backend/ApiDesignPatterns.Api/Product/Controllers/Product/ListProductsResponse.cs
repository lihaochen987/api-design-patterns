using System.ComponentModel.DataAnnotations;

namespace backend.Product.Controllers.Product;

public class ListProductsResponse
{
    [Required] public required IEnumerable<GetProductResponse> Results { get; init; } = [];
    public string? NextPageToken { get; init; }
    public int TotalCount { get; init; }
}
