// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Inventory.DomainModels.ValueObjects;

public record Quantity
{
    private Quantity()
    {
    }

    public decimal Value { get; init; }

    public Quantity(decimal value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Invalid value for quantity");
        }

        Value = value;
    }

    private static bool IsValid(decimal value)
    {
        return value >= 0;
    }
}
