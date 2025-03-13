using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

public class ListProductsResponse
{
    [Required] public required IEnumerable<GetProductResponse> Results { get; init; } = [];
    public string? NextPageToken { get; init; }
    public int TotalCount { get; init; }
}
