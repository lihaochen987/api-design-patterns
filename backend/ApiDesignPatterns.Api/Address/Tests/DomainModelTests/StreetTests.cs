// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.DomainModels.ValueObjects;
using FluentAssertions;
using Xunit;

namespace backend.Address.Tests.DomainModelTests;

public class StreetTests
{
    [Fact]
    public void Constructor_WithValidStreet_ShouldCreateInstance()
    {
        const string streetValue = "123 Main Street";

        var street = new Street(streetValue);

        street.Value.Should().Be(streetValue);
    }

    [Theory]
    [InlineData("123 Main St")]
    [InlineData("1234 Broadway Ave")]
    [InlineData("Apt 5, 101 Park Ave")]
    [InlineData("10-15 34th Street")]
    [InlineData("P.O. Box 123")]
    [InlineData("100 O'Connor Road")]
    [InlineData("42 Rue de l'Université")]
    [InlineData("221B Baker Street")]
    [InlineData("123/45 Fifth Avenue")]
    [InlineData("123 Main St #101")]
    public void Constructor_WithValidStreets_ShouldCreateInstance(string streetValue)
    {
        var street = new Street(streetValue);

        street.Value.Should().Be(streetValue);
    }

    [Theory]
    [InlineData(null, "Street cannot be null, empty, or whitespace.")]
    [InlineData("", "Street cannot be null, empty, or whitespace.")]
    [InlineData("   ", "Street cannot be null, empty, or whitespace.")]
    public void Constructor_WithNullOrEmptyValue_ShouldThrowArgumentNullException(string? streetValue,
        string expectedErrorMessage)
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new Street(streetValue!));
        exception.Message.Should().Contain(expectedErrorMessage);
    }

    [Theory]
    [InlineData("1")]
    [InlineData("12")]
    public void Constructor_WithTooShortValue_ShouldThrowArgumentOutOfRangeException(string streetValue)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Street(streetValue));
        exception.Message.Should().Contain($"Street must be at least 3 characters long.");
    }

    [Fact]
    public void Constructor_WithTooLongValue_ShouldThrowArgumentOutOfRangeException()
    {
        string streetValue = new('A', 101);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Street(streetValue));
        exception.Message.Should().Contain("Street cannot exceed 100 characters.");
    }

    [Theory]
    [InlineData("123 Main St!", "!")]
    [InlineData("123 Main St@", "@")]
    [InlineData("123 Main St$", "$")]
    [InlineData("123 Main St%", "%")]
    [InlineData("123 Main St&", "&")]
    [InlineData("123 Main St*", "*")]
    [InlineData("123 Main St()", "()")]
    [InlineData("123 Main St:;", ":;")]
    [InlineData("123 Main St<>", "<>")]
    [InlineData("123 Main St=+", "=+")]
    public void Constructor_WithInvalidCharacters_ShouldThrowArgumentException(string streetValue, string invalidChars)
    {
        var exception = Assert.Throws<ArgumentException>(() => new Street(streetValue));
        exception.Message.Should().Contain($"Street contains invalid characters: '{invalidChars}'.");
    }

    [Fact]
    public void ToString_ShouldReturnStreetValue()
    {
        const string streetValue = "123 Main Street";
        var street = new Street(streetValue);

        string result = street.ToString();

        result.Should().Be(streetValue);
    }

    [Fact]
    public void Equality_BetweenSameValues_ShouldBeTrue()
    {
        var street1 = new Street("123 Main Street");
        var street2 = new Street("123 Main Street");

        street1.Should().Be(street2);
        (street1 == street2).Should().BeTrue();
    }

    [Fact]
    public void Equality_BetweenDifferentValues_ShouldBeFalse()
    {
        var street1 = new Street("123 Main Street");
        var street2 = new Street("456 Oak Avenue");

        street1.Should().NotBe(street2);
        (street1 != street2).Should().BeTrue();
    }

    [Fact]
    public void RecordEquality_ShouldWorkCorrectly()
    {
        var street1 = new Street("123 Main Street");
        var street2 = new Street("123 Main Street");

        street1.Should().Be(street2);
        street1.Should().Be(street1);
        street1.GetHashCode().Should().Be(street2.GetHashCode());
    }

    [Fact]
    public void Constructor_WithMinimumLength_ShouldCreateInstance()
    {
        const string streetValue = "123";

        var street = new Street(streetValue);

        street.Value.Should().Be(streetValue);
    }

    [Fact]
    public void Constructor_WithMaximumLength_ShouldCreateInstance()
    {
        string streetValue = new('A', 100);

        var street = new Street(streetValue);

        street.Value.Should().Be(streetValue);
    }

    [Theory]
    [InlineData("123-456 Main St")]
    [InlineData("123/456 Main St")]
    [InlineData("123.Main St")]
    [InlineData("123,Main St")]
    [InlineData("123'Main St")]
    [InlineData("123#Main St")]
    public void Constructor_WithAllowedSpecialCharacters_ShouldCreateInstance(string streetValue)
    {
        var street = new Street(streetValue);

        street.Value.Should().Be(streetValue);
    }

    [Fact]
    public void Constructor_WithAllAllowedSpecialCharacters_ShouldCreateInstance()
    {
        const string streetValue = "123 Main-St./,'#456";

        var street = new Street(streetValue);

        street.Value.Should().Be(streetValue);
    }

    [Fact]
    public void Constructor_WithMultipleInvalidCharacters_ShouldThrowArgumentExceptionWithAllInvalidChars()
    {
        const string streetValue = "123 Main St!@#$%";

        var exception = Assert.Throws<ArgumentException>(() => new Street(streetValue));
        exception.Message.Should().Contain("Street contains invalid characters: '!@$%'.");
    }
}
