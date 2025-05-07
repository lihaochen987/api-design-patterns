// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.DomainModels.ValueObjects;

/// <summary>
/// Represents the local digits portion of a phone number with validation rules.
/// </summary>
public readonly record struct PhoneDigits
{
    // Constants for validation rules
    private const int MinDigitCount = 4;
    private const int MaxDigitCount = 10;

    /// <summary>
    /// Gets the phone digits value.
    /// </summary>
    public long Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the PhoneDigits record with validated value.
    /// </summary>
    /// <param name="value">The phone number digits.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is non-positive or has an invalid number of digits.</exception>
    public PhoneDigits(long? value)
    {
        ValidatePhoneDigits(value);
        Value = value!.Value;
    }

    /// <summary>
    /// Validates the given phone digits against constraints.
    /// </summary>
    /// <param name="value">The phone digits to validate.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is non-positive or has an invalid number of digits.</exception>
    private static void ValidatePhoneDigits(long? value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "Phone digits cannot be null.");
        }

        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value, "Phone digits must be a positive number.");
        }

        string digits = value.Value.ToString();

        switch (digits.Length)
        {
            case < MinDigitCount:
                throw new ArgumentOutOfRangeException(nameof(value), digits.Length,
                    $"Phone digits must contain at least {MinDigitCount} digits.");
            case > MaxDigitCount:
                throw new ArgumentOutOfRangeException(nameof(value), digits.Length,
                    $"Phone digits cannot contain more than {MaxDigitCount} digits.");
        }
    }

    /// <summary>
    /// Returns the string representation of the phone digits.
    /// </summary>
    /// <returns>The phone digits value as a string.</returns>
    public override string ToString()
    {
        return Value.ToString();
    }
}
