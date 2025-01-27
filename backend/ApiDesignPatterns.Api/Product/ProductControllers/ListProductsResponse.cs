using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

public class ListProductsResponse
{
    public IEnumerable<GetProductResponse> Results { get; init; } = [];
    public string? NextPageToken { get; init; }
}
