// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.DomainModels.ValueObjects;

public record Weight
{
    private Weight()
    {
    }

    public decimal Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the Weight record with validated weight.
    /// </summary>
    /// <param name="value">The weight in kilograms.</param>
    public Weight(decimal value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Invalid value for weight");
        }

        Value = value;
    }

    private static bool IsValid(decimal value)
    {
        return value is > 0 and <= 1000;
    }
}
