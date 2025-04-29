// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.DomainModels.ValueObjects;

/// <summary>
/// Represents storage instructions for a product with validation rules.
/// </summary>
public record StorageInstructions
{
    private const int MaxLength = 200;

    /// <summary>
    /// Private constructor for JSON deserialization and object mapping.
    /// </summary>
    private StorageInstructions()
    {
        Value = string.Empty;
    }

    /// <summary>
    /// Gets the storage instructions text.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the StorageInstructions record with validated value.
    /// </summary>
    /// <param name="value">The storage instructions text.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value exceeds maximum allowed length.</exception>
    public StorageInstructions(string value)
    {
        ValidateStorageInstructions(value);
        Value = value;
    }

    /// <summary>
    /// Validates the given storage instructions text against constraints.
    /// </summary>
    /// <param name="value">The storage instructions text to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value exceeds maximum allowed length.</exception>
    private static void ValidateStorageInstructions(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value),
                "Storage instructions cannot be null, empty, or whitespace.");
        }

        if (value.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value.Length,
                $"Storage instructions text cannot exceed {MaxLength} characters.");
        }
    }

    /// <summary>
    /// Returns the string representation of the storage instructions.
    /// </summary>
    /// <returns>The storage instructions text.</returns>
    public override string ToString()
    {
        return Value;
    }
}
