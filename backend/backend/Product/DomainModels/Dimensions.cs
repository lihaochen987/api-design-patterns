using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Product.DomainModels;

public class Dimensions : IValueObject
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

    [Column("ProductDimensionsLength")] public decimal Length { get; init; }
    [Column("ProductDimensionsWidth")] public decimal Width { get; init; }
    [Column("ProductDimensionsHeight")] public decimal Height { get; init; }

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