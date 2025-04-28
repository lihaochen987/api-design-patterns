// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.DomainModels.ValueObjects;

public record Rating
{
    private Rating()
    {
    }

    public decimal Value { get; init; }

    public Rating(decimal value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Invalid value for rating");
        }

        Value = value;
    }

    private static bool IsValid(decimal value)
    {
        return value is >= 1 and <= 5;
    }
}
