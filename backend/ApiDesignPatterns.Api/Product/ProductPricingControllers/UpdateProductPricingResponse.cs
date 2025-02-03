using System.ComponentModel.DataAnnotations;

namespace backend.Product.ProductPricingControllers;

public record UpdateProductPricingResponse
{
    [Required] public required string Id { get; init; }
    [Required] public required string BasePrice { get; init; }
    [Required] public required string DiscountPercentage { get; init; }
    [Required] public required string TaxRate { get; init; }
}
