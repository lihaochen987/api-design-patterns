// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.DomainModels.ValueObjects;
using FluentAssertions;
using Xunit;

namespace backend.Address.Tests.DomainModelTests;

public class PostalCodeTests
{
    [Fact]
    public void Constructor_WithValidPostalCode_ShouldCreateInstance()
    {
        const string postalCodeValue = "12345";

        var postalCode = new PostalCode(postalCodeValue);

        postalCode.Value.Should().Be(postalCodeValue);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("12345")]
    [InlineData("12345-6789")]
    [InlineData("ABC 123")]
    [InlineData("A1B 2C3")]
    [InlineData("SW1A 1AA")]
    [InlineData("90210")]
    public void Constructor_WithValidPostalCodes_ShouldCreateInstance(string postalCodeValue)
    {
        var postalCode = new PostalCode(postalCodeValue);

        postalCode.Value.Should().Be(postalCodeValue);
    }

    [Theory]
    [InlineData(null, "Postal code cannot be null, empty, or whitespace.")]
    [InlineData("", "Postal code cannot be null, empty, or whitespace.")]
    [InlineData("   ", "Postal code cannot be null, empty, or whitespace.")]
    public void Constructor_WithNullOrEmptyValue_ShouldThrowArgumentNullException(string? postalCodeValue,
        string expectedErrorMessage)
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new PostalCode(postalCodeValue!));
        exception.Message.Should().Contain(expectedErrorMessage);
    }

    [Theory]
    [InlineData("1")]
    [InlineData("12")]
    public void Constructor_WithTooShortValue_ShouldThrowArgumentOutOfRangeException(string postalCodeValue)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new PostalCode(postalCodeValue));
        exception.Message.Should().Contain($"Postal code must be at least 3 characters long.");
    }

    [Fact]
    public void Constructor_WithTooLongValue_ShouldThrowArgumentOutOfRangeException()
    {
        string postalCodeValue = new('A', 13);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new PostalCode(postalCodeValue));
        exception.Message.Should().Contain("Postal code cannot exceed 12 characters.");
    }

    [Theory]
    [InlineData("12345!", "!")]
    [InlineData("ABC@123", "@")]
    [InlineData("123#456", "#")]
    [InlineData("A1B$2C3", "$")]
    [InlineData("12345.6789", ".")]
    [InlineData("12345_6789", "_")]
    [InlineData("123:456", ":")]
    public void Constructor_WithInvalidCharacters_ShouldThrowArgumentException(string postalCodeValue,
        string invalidChars)
    {
        var exception = Assert.Throws<ArgumentException>(() => new PostalCode(postalCodeValue));
        exception.Message.Should().Contain($"Postal code contains invalid characters: '{invalidChars}'");
        exception.Message.Should().Contain("Only letters, digits, spaces, and hyphens are allowed.");
    }

    [Fact]
    public void ToString_ShouldReturnPostalCodeValue()
    {
        const string postalCodeValue = "10001";
        var postalCode = new PostalCode(postalCodeValue);

        string result = postalCode.ToString();

        result.Should().Be(postalCodeValue);
    }

    [Fact]
    public void Equality_BetweenSameValues_ShouldBeTrue()
    {
        var postalCode1 = new PostalCode("75001");
        var postalCode2 = new PostalCode("75001");

        postalCode1.Should().Be(postalCode2);
        (postalCode1 == postalCode2).Should().BeTrue();
    }

    [Fact]
    public void Equality_BetweenDifferentValues_ShouldBeFalse()
    {
        var postalCode1 = new PostalCode("75001");
        var postalCode2 = new PostalCode("75002");

        postalCode1.Should().NotBe(postalCode2);
        (postalCode1 != postalCode2).Should().BeTrue();
    }

    [Fact]
    public void RecordEquality_ShouldWorkCorrectly()
    {
        var postalCode1 = new PostalCode("EC1A 1BB");
        var postalCode2 = new PostalCode("EC1A 1BB");

        postalCode1.Should().Be(postalCode2);
        postalCode1.Should().Be(postalCode1);
        postalCode1.GetHashCode().Should().Be(postalCode2.GetHashCode());
    }

    [Fact]
    public void Constructor_WithMinimumLength_ShouldCreateInstance()
    {
        const string postalCodeValue = "123";

        var postalCode = new PostalCode(postalCodeValue);

        postalCode.Value.Should().Be(postalCodeValue);
    }

    [Fact]
    public void Constructor_WithMaximumLength_ShouldCreateInstance()
    {
        const string postalCodeValue = "123456789012";

        var postalCode = new PostalCode(postalCodeValue);

        postalCode.Value.Should().Be(postalCodeValue);
    }

    [Theory]
    [InlineData("123-456")]
    [InlineData("123 456")]
    [InlineData("A1-B2")]
    [InlineData("A1 B2")]
    public void Constructor_WithAllowedSpecialCharacters_ShouldCreateInstance(string postalCodeValue)
    {
        var postalCode = new PostalCode(postalCodeValue);

        postalCode.Value.Should().Be(postalCodeValue);
    }
}
