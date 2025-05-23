using System.ComponentModel.DataAnnotations;

namespace backend.Product.DomainModels.ValueObjects;

public class DimensionsResponse
{
    [Required] public required string Length { get; init; }
    [Required] public required string Width { get; init; }
    [Required] public required string Height { get; init; }
}
