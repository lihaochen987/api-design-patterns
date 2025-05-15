// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.DomainModels.ValueObjects;
using FluentAssertions;
using Xunit;

namespace backend.Address.Tests.DomainModelTests;

public class CountryTests
{
    [Fact]
    public void Constructor_WithValidCountry_ShouldCreateInstance()
    {
        const string countryName = "United States";

        var country = new Country(countryName);

        country.Value.Should().Be(countryName);
    }

    [Theory]
    [InlineData("US")]
    [InlineData("Canada")]
    [InlineData("United Kingdom")]
    [InlineData("Trinidad and Tobago")]
    [InlineData("Côte d'Ivoire")]
    [InlineData("Bosnia-Herzegovina")]
    [InlineData("St. Lucia")]
    [InlineData("Korea (South)")]
    public void Constructor_WithValidCountryNames_ShouldCreateInstance(string countryName)
    {
        var country = new Country(countryName);

        country.Value.Should().Be(countryName);
    }

    [Theory]
    [InlineData(null, "Country cannot be null, empty, or whitespace.")]
    [InlineData("", "Country cannot be null, empty, or whitespace.")]
    [InlineData("   ", "Country cannot be null, empty, or whitespace.")]
    public void Constructor_WithNullOrEmptyValue_ShouldThrowArgumentNullException(string? countryName,
        string expectedErrorMessage)
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new Country(countryName!));
        exception.Message.Should().Contain(expectedErrorMessage);
    }

    [Theory]
    [InlineData("A", "Country must be at least 2 characters long.")]
    public void Constructor_WithTooShortValue_ShouldThrowArgumentOutOfRangeException(string countryName,
        string expectedErrorMessage)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Country(countryName));
        exception.Message.Should().Contain(expectedErrorMessage);
    }

    [Fact]
    public void Constructor_WithTooLongValue_ShouldThrowArgumentOutOfRangeException()
    {
        string countryName = new('A', 61);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Country(countryName));
        exception.Message.Should().Contain("Country cannot exceed 60 characters.");
    }

    [Theory]
    [InlineData("United States123", "123")]
    [InlineData("Canada!", "!")]
    [InlineData("Brazil@", "@")]
    [InlineData("Germany#$%", "#$%")]
    [InlineData("Japan_", "_")]
    [InlineData("Australia+", "+")]
    public void Constructor_WithInvalidCharacters_ShouldThrowArgumentException(string countryName, string invalidChars)
    {
        var exception = Assert.Throws<ArgumentException>(() => new Country(countryName));
        exception.Message.Should().Contain($"Country contains invalid characters: '{invalidChars}'");
        exception.Message.Should().Contain("Only letters and certain special characters are allowed.");
    }

    [Fact]
    public void ToString_ShouldReturnCountryValue()
    {
        const string countryName = "France";
        var country = new Country(countryName);

        string result = country.ToString();

        result.Should().Be(countryName);
    }

    [Fact]
    public void Equality_BetweenSameValues_ShouldBeTrue()
    {
        var country1 = new Country("Mexico");
        var country2 = new Country("Mexico");

        country1.Should().Be(country2);
        (country1 == country2).Should().BeTrue();
    }

    [Fact]
    public void Equality_BetweenDifferentValues_ShouldBeFalse()
    {
        var country1 = new Country("Spain");
        var country2 = new Country("Portugal");

        country1.Should().NotBe(country2);
        (country1 != country2).Should().BeTrue();
    }

    [Fact]
    public void Constructor_WithTwoLetterCountryCode_ShouldCreateInstance()
    {
        const string countryCode = "US";

        var country = new Country(countryCode);

        country.Value.Should().Be(countryCode);
    }

    [Fact]
    public void Constructor_WithThreeLetterCountryCode_ShouldCreateInstance()
    {
        const string countryCode = "USA";

        var country = new Country(countryCode);

        country.Value.Should().Be(countryCode);
    }

    [Fact]
    public void RecordEquality_ShouldWorkCorrectly()
    {
        var country1 = new Country("Italy");
        var country2 = new Country("Italy");

        country1.Should().Be(country2);
        country1.Should().Be(country1);
        country1.GetHashCode().Should().Be(country2.GetHashCode());
    }
}
