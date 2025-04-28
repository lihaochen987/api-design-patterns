// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.DomainModels.ValueObjects;

public record SafetyWarnings
{
    private SafetyWarnings()
    {
        Value = string.Empty;
    }

    public string Value { get; init; }

    public SafetyWarnings(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Invalid value for safety warnings");
        }

        Value = value;
    }

    private static bool IsValid(string value)
    {
        return !string.IsNullOrWhiteSpace(value) && value.Length <= 300;
    }
}
