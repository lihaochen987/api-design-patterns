using System.ComponentModel.DataAnnotations;

namespace backend.Product.Controllers.Product;

public class BatchCreateProductsResponse
{
    [Required] public required IEnumerable<CreateProductResponse?> Results { get; init; } = [];
}
