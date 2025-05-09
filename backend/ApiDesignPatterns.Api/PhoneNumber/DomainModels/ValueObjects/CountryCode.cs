// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.PhoneNumber.DomainModels.ValueObjects;

/// <summary>
/// Represents a telephone country code with validation rules.
/// </summary>
public readonly record struct CountryCode
{
    private const int MinDigits = 1;
    private const int MaxDigits = 3;
    private const char PrefixChar = '+';

    /// <summary>
    /// Gets the country code value.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the CountryCode record with validated value.
    /// </summary>
    /// <param name="value">The country code.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentException">Thrown when the value doesn't start with '+' or contains non-digit characters after the prefix.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the number of digits is outside the allowed range.</exception>
    public CountryCode(string? value)
    {
        ValidateCountryCode(value);
        Value = value!;
    }

    /// <summary>
    /// Validates the given country code against constraints.
    /// </summary>
    /// <param name="value">The country code to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentException">Thrown when the value doesn't start with '+' or contains non-digit characters after the prefix.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the number of digits is outside the allowed range.</exception>
    private static void ValidateCountryCode(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "Country code cannot be null, empty, or whitespace.");
        }

        if (!value.StartsWith(PrefixChar))
        {
            throw new ArgumentException($"Country code must start with '{PrefixChar}'.", nameof(value));
        }

        string digits = value[1..];

        switch (digits.Length)
        {
            case < MinDigits:
                throw new ArgumentOutOfRangeException(nameof(value), digits.Length,
                    $"Country code must contain at least {MinDigits} digit(s) after the '{PrefixChar}' prefix.");
            case > MaxDigits:
                throw new ArgumentOutOfRangeException(nameof(value), digits.Length,
                    $"Country code cannot contain more than {MaxDigits} digits after the '{PrefixChar}' prefix.");
        }

        if (digits.All(char.IsDigit))
        {
            return;
        }

        char[] invalidChars = digits.Where(c => !char.IsDigit(c)).Distinct().ToArray();
        string invalidCharsString = new(invalidChars);
        throw new ArgumentException(
            $"Country code contains invalid characters: '{invalidCharsString}'. Only digits are allowed after the '{PrefixChar}' prefix.",
            nameof(value));
    }

    /// <summary>
    /// Returns the string representation of the country code.
    /// </summary>
    /// <returns>The country code value.</returns>
    public override string ToString()
    {
        return Value;
    }
}
