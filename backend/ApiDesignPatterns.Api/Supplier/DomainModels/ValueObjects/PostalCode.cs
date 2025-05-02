// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.DomainModels.ValueObjects;

/// <summary>
/// Represents a postal code with validation rules.
/// </summary>
public record PostalCode
{
    private const int MinLength = 3;
    private const int MaxLength = 12;

    /// <summary>
    /// Private constructor for JSON deserialization and object mapping.
    /// </summary>
    private PostalCode()
    {
        Value = string.Empty;
    }

    /// <summary>
    /// Gets the postal code value.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the PostalCode record with validated value.
    /// </summary>
    /// <param name="value">The postal code.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value length is outside the allowed range.</exception>
    /// <exception cref="ArgumentException">Thrown when the value contains invalid characters.</exception>
    public PostalCode(string value)
    {
        ValidatePostalCode(value);
        Value = value;
    }

    /// <summary>
    /// Validates the given postal code against constraints.
    /// </summary>
    /// <param name="value">The postal code to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value length is outside the allowed range.</exception>
    /// <exception cref="ArgumentException">Thrown when the value contains invalid characters.</exception>
    private static void ValidatePostalCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "Postal code cannot be null, empty, or whitespace.");
        }

        switch (value.Length)
        {
            case < MinLength:
                throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                    $"Postal code must be at least {MinLength} characters long.");
            case > MaxLength:
                throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                    $"Postal code cannot exceed {MaxLength} characters.");
        }

        const string allowedSpecialChars = "- ";
        if (value.All(c => char.IsLetterOrDigit(c) || allowedSpecialChars.Contains(c)))
        {
            return;
        }

        char[] invalidChars = value.Where(c => !char.IsLetterOrDigit(c) && !allowedSpecialChars.Contains(c)).Distinct()
            .ToArray();
        string invalidCharsString = new(invalidChars);
        throw new ArgumentException(
            $"Postal code contains invalid characters: '{invalidCharsString}'. Only letters, digits, spaces, and hyphens are allowed.",
            nameof(value));
    }

    /// <summary>
    /// Returns the string representation of the postal code.
    /// </summary>
    /// <returns>The postal code value.</returns>
    public override string ToString()
    {
        return Value;
    }
}
