// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.DomainModels.ValueObjects;

/// <summary>
/// Represents a country with validation rules.
/// </summary>
public readonly record struct Country
{
    private const int MinLength = 2;
    private const int MaxLength = 60;

    /// <summary>
    /// Gets the country value.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the Country record with validated value.
    /// </summary>
    /// <param name="value">The country name or code.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value length is outside the allowed range.</exception>
    /// <exception cref="ArgumentException">Thrown when the value contains invalid characters.</exception>
    public Country(string value)
    {
        ValidateCountry(value);
        Value = value;
    }

    /// <summary>
    /// Validates the given country against constraints.
    /// </summary>
    /// <param name="value">The country to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value length is outside the allowed range.</exception>
    /// <exception cref="ArgumentException">Thrown when the value contains invalid characters.</exception>
    private static void ValidateCountry(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "Country cannot be null, empty, or whitespace.");
        }

        switch (value.Length)
        {
            case < MinLength:
                throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                    $"Country must be at least {MinLength} characters long.");
            case > MaxLength:
                throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                    $"Country cannot exceed {MaxLength} characters.");
        }

        const string allowedSpecialChars = " -'.()";
        if (value.All(c => char.IsLetter(c) || allowedSpecialChars.Contains(c)))
        {
            return;
        }

        char[] invalidChars = value.Where(c => !char.IsLetter(c) && !allowedSpecialChars.Contains(c)).Distinct()
            .ToArray();
        string invalidCharsString = new(invalidChars);
        throw new ArgumentException(
            $"Country contains invalid characters: '{invalidCharsString}'. Only letters and certain special characters are allowed.",
            nameof(value));
    }

    /// <summary>
    /// Returns the string representation of the country.
    /// </summary>
    /// <returns>The country value.</returns>
    public override string ToString()
    {
        return Value;
    }
}
