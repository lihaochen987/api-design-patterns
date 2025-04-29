// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;

namespace backend.Review.DomainModels.ValueObjects;

/// <summary>
/// Represents a product rating with validation rules for acceptable values.
/// </summary>
public record Rating
{
    // Constants for validation rules
    private const decimal MinRating = 1m;
    private const decimal MaxRating = 5m;

    /// <summary>
    /// Private constructor for JSON deserialization and object mapping.
    /// </summary>
    private Rating()
    {
    }

    /// <summary>
    /// Gets the rating value.
    /// </summary>
    /// <value>The numeric rating value</value>
    public decimal Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the Rating record with validated value.
    /// </summary>
    /// <param name="value">The rating value (between 1 and 5).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is outside the valid range of 1 to 5.</exception>
    public Rating(decimal value)
    {
        ValidateRating(value);
        Value = value;
    }

    /// <summary>
    /// Validates the given rating against constraints.
    /// </summary>
    /// <param name="value">The rating to validate.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is outside the valid range of 1 to 5.</exception>
    private static void ValidateRating(decimal value)
    {
        switch (value)
        {
            case < MinRating:
                throw new ArgumentOutOfRangeException(nameof(value), value, $"Rating cannot be less than {MinRating}.");
            case > MaxRating:
                throw new ArgumentOutOfRangeException(nameof(value), value, $"Rating cannot exceed {MaxRating}.");
        }
    }

    /// <summary>
    /// Returns the string representation of the rating.
    /// </summary>
    /// <returns>The rating value as a string using invariant culture.</returns>
    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
}
