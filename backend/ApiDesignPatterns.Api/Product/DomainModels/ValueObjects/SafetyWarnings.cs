// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.DomainModels.ValueObjects;

/// <summary>
/// Represents the safety warnings for a product with validation rules.
/// </summary>
public readonly record struct SafetyWarnings
{
    private const int MaxLength = 300;

    /// <summary>
    /// Gets the safety warnings text.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the SafetyWarnings record with validated value.
    /// </summary>
    /// <param name="value">The safety warnings text.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value exceeds maximum allowed length.</exception>
    public SafetyWarnings(string value)
    {
        ValidateSafetyWarnings(value);
        Value = value;
    }

    /// <summary>
    /// Validates the given safety warnings text against constraints.
    /// </summary>
    /// <param name="value">The safety warnings text to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value exceeds maximum allowed length.</exception>
    private static void ValidateSafetyWarnings(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "Safety warnings cannot be null, empty, or whitespace.");
        }

        if (value.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                $"Safety warnings text cannot exceed {MaxLength} characters.");
        }
    }

    /// <summary>
    /// Returns the string representation of the safety warnings.
    /// </summary>
    /// <returns>The safety warnings text.</returns>
    public override string ToString()
    {
        return Value;
    }
}
