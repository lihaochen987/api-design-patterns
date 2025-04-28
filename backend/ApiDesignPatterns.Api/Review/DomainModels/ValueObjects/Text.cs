// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.DomainModels.ValueObjects;

public record Text
{
    private Text()
    {
        Value = string.Empty;
    }

    public string Value { get; init; }

    public Text(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Invalid value for text");
        }

        Value = value;
    }

    private static bool IsValid(string value)
    {
        return !string.IsNullOrWhiteSpace(value) && value.Length <= 2000;
    }
}
