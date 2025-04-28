// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.DomainModels.ValueObjects;

public record Name
{
    private Name()
    {
        Value = string.Empty;
    }

    public string Value { get; init; }

    public Name(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Invalid value for name");
        }

        Value = value;
    }

    private static bool IsValid(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
        {
            return false;
        }

        return value.All(c => char.IsLetter(c) || c == ' ' || c == '-' || c == '\'');
    }
}
