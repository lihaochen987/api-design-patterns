using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.DomainModels;

public record Product
{
    public required long Id { get; init; }

    public required Name Name { get; init; }

    public Category Category { get; init; }

    public required Pricing Pricing { get; init; }

    public required Dimensions Dimensions { get; init; }
}
