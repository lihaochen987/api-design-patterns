// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.DomainModels.ValueObjects;

/// <summary>
/// Represents a street address with validation rules.
/// </summary>
public readonly record struct Street
{
    private const int MinLength = 3;
    private const int MaxLength = 100;

    /// <summary>
    /// Gets the street value.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the Street record with validated value.
    /// </summary>
    /// <param name="value">The street address.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value length is outside the allowed range.</exception>
    /// <exception cref="ArgumentException">Thrown when the value contains invalid characters.</exception>
    public Street(string value)
    {
        ValidateStreet(value);
        Value = value;
    }

    /// <summary>
    /// Validates the given street against constraints.
    /// </summary>
    /// <param name="value">The street to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value length is outside the allowed range.</exception>
    /// <exception cref="ArgumentException">Thrown when the value contains invalid characters.</exception>
    private static void ValidateStreet(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "Street cannot be null, empty, or whitespace.");
        }

        switch (value.Length)
        {
            case < MinLength:
                throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                    $"Street must be at least {MinLength} characters long.");
            case > MaxLength:
                throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                    $"Street cannot exceed {MaxLength} characters.");
        }

        const string allowedSpecialChars = " -/.,'#";
        if (value.All(c => char.IsLetterOrDigit(c) || allowedSpecialChars.Contains(c)))
        {
            return;
        }

        char[] invalidChars = value.Where(c => !char.IsLetterOrDigit(c) && !allowedSpecialChars.Contains(c)).Distinct()
            .ToArray();
        string invalidCharsString = new(invalidChars);
        throw new ArgumentException(
            $"Street contains invalid characters: '{invalidCharsString}'.",
            nameof(value));
    }

    /// <summary>
    /// Returns the string representation of the street.
    /// </summary>
    /// <returns>The street value.</returns>
    public override string ToString()
    {
        return Value;
    }
}
