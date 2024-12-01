using System.ComponentModel.DataAnnotations;

namespace backend.Product.Contracts;

public class ProductListResult<T>
{
    [Required] public List<T> Items { get; init; } = [];
    [Required] public string? NextPageToken { get; init; }
}
