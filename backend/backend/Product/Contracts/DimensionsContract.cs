using System.ComponentModel.DataAnnotations;

namespace backend.Product.Contracts;

public class DimensionsContract
{
    [Required] public string Length { get; init; } = "";
    [Required] public string Width { get; init; } = "";
    [Required] public string Height { get; init; } = "";
}
