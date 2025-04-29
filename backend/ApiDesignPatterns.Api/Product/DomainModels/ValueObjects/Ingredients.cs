// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.DomainModels.ValueObjects;

/// <summary>
/// Represents the ingredients of a product with validation rules.
/// </summary>
public record Ingredients
{
    private const int MaxLength = 500;

    /// <summary>
    /// Private constructor for JSON deserialization and object mapping.
    /// </summary>
    private Ingredients()
    {
        Value = string.Empty;
    }

    /// <summary>
    /// Gets the ingredients description.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the Ingredients record with validated value.
    /// </summary>
    /// <param name="value">The ingredients description.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value exceeds maximum allowed length.</exception>
    public Ingredients(string value)
    {
        ValidateIngredients(value);
        Value = value;
    }

    /// <summary>
    /// Validates the given ingredients value against constraints.
    /// </summary>
    /// <param name="value">The ingredients value to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value exceeds maximum allowed length.</exception>
    private static void ValidateIngredients(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "Ingredients cannot be null, empty, or whitespace.");
        }

        if (value.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                $"Ingredients description cannot exceed {MaxLength} characters.");
        }
    }

    /// <summary>
    /// Returns the string representation of the ingredients.
    /// </summary>
    /// <returns>The ingredients value.</returns>
    public override string ToString()
    {
        return Value;
    }
}
