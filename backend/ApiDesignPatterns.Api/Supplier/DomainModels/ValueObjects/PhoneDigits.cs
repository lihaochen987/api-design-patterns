// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.DomainModels.ValueObjects;

public record PhoneDigits
{
    private PhoneDigits()
    {
        Value = 0;
    }

    public long Value { get; init; }

    public PhoneDigits(long value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Invalid value for phone number digits");
        }

        Value = value;
    }

    private static bool IsValid(long value)
    {
        string digits = value.ToString();
        return value > 0 && digits.Length is >= 4 and <= 10;
    }
}
