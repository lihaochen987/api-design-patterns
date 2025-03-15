namespace backend.Product.DomainModels.ValueObjects;

/// <summary>
/// Represents the physical dimensions of a product with validation rules for length, width, and height.
/// </summary>
public record Dimensions
{
    /// <summary>
    /// Private constructor for JSON deserialization and object mapping.
    /// </summary>
    private Dimensions()
    {
    }

    /// <summary>
    /// Gets the length of the product.
    /// </summary>
    /// <value>The length in cm</value>
    public decimal Length { get; init; }

    /// <summary>
    /// Gets the width of the product.
    /// </summary>
    /// <value>The width in cm</value>
    public decimal Width { get; init; }

    /// <summary>
    /// Gets the height of the product.
    /// </summary>
    /// <value>The height in cm</value>
    public decimal Height { get; init; }

    /// <summary>
    /// Initializes a new instance of the Dimensions record with validated measurements.
    /// </summary>
    /// <param name="length">The length in cm.</param>
    /// <param name="width">The width in cm.</param>
    /// <param name="height">The height in cm.</param>
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

    /// <summary>
    /// Validates the given dimensions against size and volume constraints.
    /// </summary>
    /// <param name="length">The length to validate.</param>
    /// <param name="width">The width to validate.</param>
    /// <param name="height">The height to validate.</param>
    /// <returns>True if all dimensions are within valid ranges and volume is acceptable; otherwise false.</returns>
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
