// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Globalization;

namespace backend.Product.DomainModels.ValueObjects;

/// <summary>
/// Represents the weight of a product with validation rules.
/// </summary>
public readonly record struct Weight
{
    private const decimal MinWeight = 0m;
    private const decimal MaxWeight = 1000m;

    /// <summary>
    /// Gets the weight value.
    /// </summary>
    /// <value>The weight in kilograms</value>
    public decimal Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the Weight record with validated weight.
    /// </summary>
    /// <param name="value">The weight in kilograms.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the weight is less than or equal to zero or exceeds maximum allowed value.</exception>
    public Weight(decimal? value)
    {
        ValidateWeight(value);
        Value = (decimal)value!;
    }

    /// <summary>
    /// Validates the given weight against constraints.
    /// </summary>
    /// <param name="value">The weight to validate.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the weight is less than or equal to zero or exceeds maximum allowed value.</exception>
    private static void ValidateWeight(decimal? value)
    {
        switch (value)
        {
            case null:
                throw new NullReferenceException("Value cannot be null");
            case <= MinWeight:
                throw new ArgumentOutOfRangeException(nameof(value), value,
                    $"Weight must be greater than {MinWeight} kg.");
            case > MaxWeight:
                throw new ArgumentOutOfRangeException(nameof(value), value, $"Weight cannot exceed {MaxWeight} kg.");
        }
    }

    /// <summary>
    /// Returns the string representation of the weight.
    /// </summary>
    /// <returns>The weight value as a string using invariant culture.</returns>
    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
}
