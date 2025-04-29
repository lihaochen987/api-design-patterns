// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.DomainModels.ValueObjects;

/// <summary>
/// Represents a telephone area code with validation rules.
/// </summary>
public record AreaCode
{
    private const int MinLength = 2;
    private const int MaxLength = 5;

    /// <summary>
    /// Private constructor for JSON deserialization and object mapping.
    /// </summary>
    private AreaCode()
    {
        Value = string.Empty;
    }

    /// <summary>
    /// Gets the area code value.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the AreaCode record with validated value.
    /// </summary>
    /// <param name="value">The area code.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value length is outside the allowed range.</exception>
    /// <exception cref="ArgumentException">Thrown when the value contains non-digit characters.</exception>
    public AreaCode(string value)
    {
        ValidateAreaCode(value);
        Value = value;
    }

    /// <summary>
    /// Validates the given area code against constraints.
    /// </summary>
    /// <param name="value">The area code to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value length is outside the allowed range.</exception>
    /// <exception cref="ArgumentException">Thrown when the value contains non-digit characters.</exception>
    private static void ValidateAreaCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "Area code cannot be null, empty, or whitespace.");
        }

        switch (value.Length)
        {
            case < MinLength:
                throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                    $"Area code must be at least {MinLength} characters long.");
            case > MaxLength:
                throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                    $"Area code cannot exceed {MaxLength} characters.");
        }

        if (value.All(char.IsDigit))
        {
            return;
        }

        char[] invalidChars = value.Where(c => !char.IsDigit(c)).Distinct().ToArray();
        string invalidCharsString = new(invalidChars);
        throw new ArgumentException(
            $"Area code contains invalid characters: '{invalidCharsString}'. Only digits are allowed.",
            nameof(value));
    }

    /// <summary>
    /// Returns the string representation of the area code.
    /// </summary>
    /// <returns>The area code value.</returns>
    public override string ToString()
    {
        return Value;
    }
}
