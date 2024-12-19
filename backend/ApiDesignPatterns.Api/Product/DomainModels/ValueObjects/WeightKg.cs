// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.DomainModels.ValueObjects;

public record WeightKg
{
    public decimal Value { get; init; }

    public WeightKg(decimal value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Invalid value for WeightKg");
        }

        Value = value;
    }

    private static bool IsValid(decimal weight)
    {
        return weight > 0;
    }
}
