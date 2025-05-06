// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.DomainModels.ValueObjects;

/// <summary>
/// Represents a city name with validation rules.
/// </summary>
public readonly record struct City
{
    private const int MinLength = 2;
    private const int MaxLength = 50;

    /// <summary>
    /// Gets the city name value.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the City record with validated value.
    /// </summary>
    /// <param name="value">The city name.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value length is outside the allowed range.</exception>
    /// <exception cref="ArgumentException">Thrown when the value contains invalid characters.</exception>
    public City(string value)
    {
        ValidateCity(value);
        Value = value;
    }

    /// <summary>
    /// Validates the given city name against constraints.
    /// </summary>
    /// <param name="value">The city name to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value length is outside the allowed range.</exception>
    /// <exception cref="ArgumentException">Thrown when the value contains invalid characters.</exception>
    private static void ValidateCity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "City name cannot be null, empty, or whitespace.");
        }

        switch (value.Length)
        {
            case < MinLength:
                throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                    $"City name must be at least {MinLength} characters long.");
            case > MaxLength:
                throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                    $"City name cannot exceed {MaxLength} characters.");
        }

        const string allowedSpecialChars = " -'.";
        if (value.All(c => char.IsLetter(c) || allowedSpecialChars.Contains(c)))
        {
            return;
        }

        char[] invalidChars = value.Where(c => !char.IsLetter(c) && !allowedSpecialChars.Contains(c)).Distinct()
            .ToArray();
        string invalidCharsString = new(invalidChars);
        throw new ArgumentException(
            $"City name contains invalid characters: '{invalidCharsString}'. Only letters and certain special characters are allowed.",
            nameof(value));
    }

    /// <summary>
    /// Returns the string representation of the city name.
    /// </summary>
    /// <returns>The city name value.</returns>
    public override string ToString()
    {
        return Value;
    }
}
