using backend.Models;

namespace backend.Contracts;

public class UpdateProductRequest
{
    public Product? Resource { get; init; }
}