using backend.Models;

namespace backend.Contracts;

public class CreateProductRequest
{
    public Product? Resource { get; init; }
}