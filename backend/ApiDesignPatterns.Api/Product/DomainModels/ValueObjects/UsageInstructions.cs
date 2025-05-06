// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.DomainModels.ValueObjects;

/// <summary>
/// Represents usage instructions for a product with validation rules.
/// </summary>
public readonly record struct UsageInstructions
{
    // Constant for validation rule
    private const int MaxLength = 500;

    /// <summary>
    /// Gets the usage instructions text.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the UsageInstructions record with validated value.
    /// </summary>
    /// <param name="value">The usage instructions text.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value exceeds maximum allowed length.</exception>
    public UsageInstructions(string value)
    {
        ValidateUsageInstructions(value);
        Value = value;
    }

    /// <summary>
    /// Validates the given usage instructions text against constraints.
    /// </summary>
    /// <param name="value">The usage instructions text to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value exceeds maximum allowed length.</exception>
    private static void ValidateUsageInstructions(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "Usage instructions cannot be null, empty, or whitespace.");
        }

        if (value.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                $"Usage instructions text cannot exceed {MaxLength} characters.");
        }
    }

    /// <summary>
    /// Returns the string representation of the usage instructions.
    /// </summary>
    /// <returns>The usage instructions text.</returns>
    public override string ToString()
    {
        return Value;
    }
}
