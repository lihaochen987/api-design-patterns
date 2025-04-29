// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.DomainModels.ValueObjects;

public record AreaCode
{
    private AreaCode()
    {
        Value = string.Empty;
    }

    public string Value { get; init; }

    public AreaCode(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Invalid value for area code");
        }

        Value = value;
    }

    private static bool IsValid(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return value.Length is >= 2 and <= 5 && value.All(char.IsDigit);
    }

    public override string ToString()
    {
        return Value;
    }
}
