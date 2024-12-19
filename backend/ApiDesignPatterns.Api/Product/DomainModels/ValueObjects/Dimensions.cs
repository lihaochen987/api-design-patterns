namespace backend.Product.DomainModels.ValueObjects;

public record Dimensions
{
    public decimal Length { get; init; }
    public decimal Width { get; init; }
    public decimal Height { get; init; }

    public Dimensions(decimal length, decimal width, decimal height)
    {
        if (!IsValid(length, width, height))
        {
            throw new ArgumentException("Invalid value for dimensions");
        }

        Length = length;
        Width = width;
        Height = height;
    }

    private static bool IsValid(decimal length, decimal width, decimal height)
    {
        if (length is < 0 or > 100)
        {
            return false;
        }

        if (width is < 0 or > 50)
        {
            return false;
        }

        if (height is < 0 or > 50)
        {
            return false;
        }

        return length * width * height <= 110000;
    }
}
