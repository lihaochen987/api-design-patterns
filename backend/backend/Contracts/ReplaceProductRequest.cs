using backend.Models;

namespace backend.Contracts;

public class ReplaceProductRequest
{
    public Product? Resource { get; init; }
}