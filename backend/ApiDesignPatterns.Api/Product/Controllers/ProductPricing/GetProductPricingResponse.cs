using System.ComponentModel.DataAnnotations;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.Controllers.ProductPricing;

public record GetProductPricingResponse
{
    [Required] public required string Id { get; init; }
    [Required] public required ProductPricingResponse pricing { get; init; }
}
