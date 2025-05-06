// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Globalization;

namespace backend.Inventory.DomainModels.ValueObjects;

/// <summary>
/// Represents the quantity of an inventory item with validation rules.
/// </summary>
public readonly record struct Quantity
{
    // Constant for validation rule
    private const decimal MinQuantity = 0m;

    /// <summary>
    /// Gets the quantity value.
    /// </summary>
    public decimal Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the Quantity class with validated value.
    /// </summary>
    /// <param name="value">The quantity value.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is negative.</exception>
    public Quantity(decimal value)
    {
        ValidateQuantity(value);
        Value = value;
    }

    /// <summary>
    /// Validates the given quantity against constraints.
    /// </summary>
    /// <param name="value">The quantity to validate.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is negative.</exception>
    private static void ValidateQuantity(decimal value)
    {
        if (value < MinQuantity)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value, $"Quantity cannot be negative.");
        }
    }

    /// <summary>
    /// Returns the string representation of the quantity.
    /// </summary>
    /// <returns>The quantity value as a string using invariant culture.</returns>
    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
}
