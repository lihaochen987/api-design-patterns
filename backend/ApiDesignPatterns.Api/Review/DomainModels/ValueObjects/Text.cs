// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.DomainModels.ValueObjects;

/// <summary>
/// Represents the text content of a review with validation rules.
/// </summary>
public record Text
{
    // Constant for validation rule
    private const int MaxLength = 2000;

    /// <summary>
    /// Private constructor for JSON deserialization and object mapping.
    /// </summary>
    private Text()
    {
        Value = string.Empty;
    }

    /// <summary>
    /// Gets the text content.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the Text record with validated content.
    /// </summary>
    /// <param name="value">The text content.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value exceeds maximum allowed length.</exception>
    public Text(string value)
    {
        ValidateText(value);
        Value = value;
    }

    /// <summary>
    /// Validates the given text content against constraints.
    /// </summary>
    /// <param name="value">The text content to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value exceeds maximum allowed length.</exception>
    private static void ValidateText(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "Review text cannot be null, empty, or whitespace.");
        }

        if (value.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                $"Review text cannot exceed {MaxLength} characters.");
        }
    }

    /// <summary>
    /// Returns the string representation of the text content.
    /// </summary>
    /// <returns>The text content.</returns>
    public override string ToString()
    {
        return Value;
    }
}
