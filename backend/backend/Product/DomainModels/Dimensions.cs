namespace backend.Product.DomainModels;

public record Dimensions
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

    public decimal Length { get; init; }
    public decimal Width { get; init; }
    public decimal Height { get; init; }

    private static void EnforceInvariants(decimal length, decimal width, decimal height)
    {
        if (length is <= 0 or >= 100)
            throw new ArgumentException("Length must be greater than zero and less than or equal to 100cm.");
        if (width is <= 0 or >= 50)
            throw new ArgumentException("Width must be greater than zero and less than or equal to 50cm.");
        if (height is <= 0 or >= 50)
            throw new ArgumentException("Height must be greater than zero and less than or equal to 50cm.");
        if (width * length * height > 110000)
            throw new ArgumentException("Total volume must be less than 110,000cm");
    }
}