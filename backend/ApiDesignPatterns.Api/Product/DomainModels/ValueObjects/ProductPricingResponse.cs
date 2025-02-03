using System.ComponentModel.DataAnnotations;

namespace backend.Product.DomainModels.ValueObjects;

public class ProductPricingResponse
{
    [Required] public required string BasePrice { get; init; }
    [Required] public required string DiscountPercentage { get; init; }
    [Required] public required string TaxRate { get; init; }
}
