using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Product.DomainModels;

public class Dimensions
{
    public Dimensions(
        decimal length,
        decimal width,
        decimal height)
    {
        EnforceInvariants(length, width, height);
        Length = length;
        Width = width;
        Height = height;
    }

    [Column("product_dimensions_length")] public decimal Length { get; init; }
    [Column("product_dimensions_width")] public decimal Width { get; init; }
    [Column("product_dimensions_height")] public decimal Height { get; init; }

    private static void EnforceInvariants(decimal length, decimal width, decimal height)
    {
        if (length <= 0)
            throw new ArgumentException("Length must be greater than zero.");
        if (width <= 0)
            throw new ArgumentException("Width must be greater than zero.");
        if (height <= 0)
            throw new ArgumentException("Height must be greater than zero.");
    }
}