using System.ComponentModel.DataAnnotations;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.DomainModels.Views;

public record ProductPricingView
{
    [Required] public required long Id { get; init; }
    [Required] public required Pricing Pricing { get; init; }
}
