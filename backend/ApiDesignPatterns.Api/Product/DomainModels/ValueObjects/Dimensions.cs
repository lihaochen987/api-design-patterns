namespace backend.Product.DomainModels.ValueObjects;

/// <summary>
/// Represents the physical dimensions of a product with validation rules for length, width, and height.
/// </summary>
public record Dimensions
{
    private const decimal MaxLength = 100m;
    private const decimal MaxWidth = 50m;
    private const decimal MaxHeight = 50m;
    private const decimal MaxVolume = 110000m;

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
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any dimension is negative or exceeds maximum allowed value.</exception>
    /// <exception cref="ArgumentException">Thrown when the total volume exceeds maximum allowed volume.</exception>
    public Dimensions(decimal length, decimal width, decimal height)
    {
        ValidateDimensions(length, width, height);

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
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any dimension is negative or exceeds maximum allowed value.</exception>
    /// <exception cref="ArgumentException">Thrown when the total volume exceeds maximum allowed volume.</exception>
    private static void ValidateDimensions(decimal length, decimal width, decimal height)
    {
        switch (length)
        {
            case < 0:
                throw new ArgumentOutOfRangeException(nameof(length), length, "Length cannot be negative.");
            case > MaxLength:
                throw new ArgumentOutOfRangeException(nameof(length), length, $"Length cannot exceed {MaxLength} cm.");
        }

        switch (width)
        {
            case < 0:
                throw new ArgumentOutOfRangeException(nameof(width), width, "Width cannot be negative.");
            case > MaxWidth:
                throw new ArgumentOutOfRangeException(nameof(width), width, $"Width cannot exceed {MaxWidth} cm.");
        }

        switch (height)
        {
            case < 0:
                throw new ArgumentOutOfRangeException(nameof(height), height, "Height cannot be negative.");
            case > MaxHeight:
                throw new ArgumentOutOfRangeException(nameof(height), height, $"Height cannot exceed {MaxHeight} cm.");
        }

        decimal volume = length * width * height;
        if (volume > MaxVolume)
        {
            throw new ArgumentException(
                $"Total volume ({volume} cm³) exceeds maximum allowed volume of {MaxVolume} cm³.",
                $"{nameof(length)}, {nameof(width)}, {nameof(height)}");
        }
    }
}
