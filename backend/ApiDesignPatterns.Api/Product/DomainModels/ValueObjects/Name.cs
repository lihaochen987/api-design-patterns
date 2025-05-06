// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.DomainModels.ValueObjects;

/// <summary>
/// Represents a validated product name with specific formatting rules.
/// </summary>
public readonly record struct Name
{
    private const int MaxLength = 50;
    private const string ValidSpecialChars = " -'";

    /// <summary>
    /// Gets the name value.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the Name record with validated value.
    /// </summary>
    /// <param name="value">The name value.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value exceeds maximum allowed length.</exception>
    /// <exception cref="ArgumentException">Thrown when the value contains invalid characters.</exception>
    public Name(string value)
    {
        ValidateName(value);
        Value = value;
    }

    /// <summary>
    /// Validates the given name value against constraints.
    /// </summary>
    /// <param name="value">The name value to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value exceeds maximum allowed length.</exception>
    /// <exception cref="ArgumentException">Thrown when the value contains invalid characters.</exception>
    private static void ValidateName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "Name cannot be null, empty, or whitespace.");
        }

        if (value.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                $"Name cannot exceed {MaxLength} characters.");
        }

        char[] invalidChars = value.Where(c => !char.IsLetter(c) && !ValidSpecialChars.Contains(c)).ToArray();
        if (invalidChars.Length == 0)
        {
            return;
        }

        string invalidCharsString = new(invalidChars.Distinct().ToArray());
        throw new ArgumentException(
            $"Name contains invalid characters: '{invalidCharsString}'. Only letters, spaces, hyphens, and apostrophes are allowed.",
            nameof(value));
    }

    /// <summary>
    /// Returns the string representation of the name.
    /// </summary>
    /// <returns>The name value.</returns>
    public override string ToString()
    {
        return Value;
    }
}
