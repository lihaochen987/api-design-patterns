// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.User.DomainModels.ValueObjects;

/// <summary>
/// Represents an email address with validation rules.
/// </summary>
public readonly record struct Email
{
    private const int MaxLength = 254;

    /// <summary>
    /// Gets the email address value.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the Email record with validated value.
    /// </summary>
    /// <param name="value">The email address.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value length exceeds the allowed limit.</exception>
    /// <exception cref="ArgumentException">Thrown when the value is not a valid email format.</exception>
    public Email(string value)
    {
        ValidateEmail(value);
        Value = value;
    }

    /// <summary>
    /// Validates the given email address against constraints.
    /// </summary>
    /// <param name="value">The email address to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value length exceeds the allowed limit.</exception>
    /// <exception cref="ArgumentException">Thrown when the value is not a valid email format.</exception>
    private static void ValidateEmail(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "Email address cannot be null, empty, or whitespace.");
        }

        if (value.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                $"Email address cannot exceed {MaxLength} characters.");
        }

        try
        {
            // Use .NET's built-in email validation via MailAddress
            // This validates the basic format of the email address
            var mailAddress = new System.Net.Mail.MailAddress(value);

            // Ensure the email address created matches the input value
            if (mailAddress.Address != value)
            {
                throw new ArgumentException("Email address is not in a valid format.", nameof(value));
            }
        }
        catch (FormatException)
        {
            throw new ArgumentException("Email address is not in a valid format.", nameof(value));
        }

        // Additional validation rules could be added here if needed
        // For example, checking for specific domains, etc.
    }

    /// <summary>
    /// Returns the string representation of the email address.
    /// </summary>
    /// <returns>The email address value.</returns>
    public override string ToString()
    {
        return Value;
    }
}
