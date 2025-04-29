// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.DomainModels.ValueObjects;

public record CountryCode
{
    private CountryCode()
    {
        Value = string.Empty;
    }

    public string Value { get; init; }

    public CountryCode(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Invalid value for country code");
        }

        Value = value;
    }

    private static bool IsValid(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        // Country codes typically start with + and contain 1-3 digits
        if (!value.StartsWith('+'))
        {
            return false;
        }

        string digits = value[1..];
        return digits.Length is >= 1 and <= 3 && digits.All(char.IsDigit);
    }
}
