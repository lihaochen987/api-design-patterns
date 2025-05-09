// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.DomainModels.ValueObjects;

/// <summary>
/// Represents a first name with validation rules.
/// </summary>
public readonly record struct FirstName
{
    private const int MinLength = 1;
    private const int MaxLength = 50;

    /// <summary>
    /// Gets the first name value.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the FirstName record with validated value.
    /// </summary>
    /// <param name="value">The first name.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value length is outside the allowed range.</exception>
    /// <exception cref="ArgumentException">Thrown when the value contains invalid characters.</exception>
    public FirstName(string value)
    {
        ValidateFirstName(value);
        Value = value;
    }

    /// <summary>
    /// Validates the given first name against constraints.
    /// </summary>
    /// <param name="value">The first name to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value length is outside the allowed range.</exception>
    /// <exception cref="ArgumentException">Thrown when the value contains invalid characters.</exception>
    private static void ValidateFirstName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "First name cannot be null, empty, or whitespace.");
        }

        switch (value.Length)
        {
            case < MinLength:
                throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                    $"First name must be at least {MinLength} characters long.");
            case > MaxLength:
                throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                    $"First name cannot exceed {MaxLength} characters.");
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
            $"First name contains invalid characters: '{invalidCharsString}'. Only letters and certain special characters are allowed.",
            nameof(value));
    }

    /// <summary>
    /// Returns the string representation of the first name.
    /// </summary>
    /// <returns>The first name value.</returns>
    public override string ToString()
    {
        return Value;
    }
}
