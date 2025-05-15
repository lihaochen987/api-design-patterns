// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.DomainModels.ValueObjects;
using FluentAssertions;
using Xunit;

namespace backend.Address.Tests.DomainModelTests;

public class CityTests
{
    [Fact]
    public void Constructor_WithValidCity_ShouldCreateInstance()
    {
        const string cityName = "New York";

        var city = new City(cityName);

        city.Value.Should().Be(cityName);
    }

    [Theory]
    [InlineData("San Francisco")]
    [InlineData("O'Hare")]
    [InlineData("Winston-Salem")]
    [InlineData("St. Louis")]
    [InlineData("Stratford-upon-Avon")]
    public void Constructor_WithSpecialCharacters_ShouldCreateInstance(string cityName)
    {
        var city = new City(cityName);

        city.Value.Should().Be(cityName);
    }

    [Theory]
    [InlineData(null, "City name cannot be null, empty, or whitespace.")]
    [InlineData("", "City name cannot be null, empty, or whitespace.")]
    [InlineData("   ", "City name cannot be null, empty, or whitespace.")]
    public void Constructor_WithNullOrEmptyValue_ShouldThrowArgumentNullException(string? cityName,
        string expectedErrorMessage)
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new City(cityName!));
        exception.Message.Should().Contain(expectedErrorMessage);
    }

    [Theory]
    [InlineData("A", "City name must be at least 2 characters long.")]
    public void Constructor_WithTooShortValue_ShouldThrowArgumentOutOfRangeException(string cityName,
        string expectedErrorMessage)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new City(cityName));
        exception.Message.Should().Contain(expectedErrorMessage);
    }

    [Fact]
    public void Constructor_WithTooLongValue_ShouldThrowArgumentOutOfRangeException()
    {
        string cityName = new('A', 51);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new City(cityName));
        exception.Message.Should().Contain("City name cannot exceed 50 characters.");
    }

    [Theory]
    [InlineData("New York123", "123")]
    [InlineData("Boston!", "!")]
    [InlineData("San@Francisco", "@")]
    [InlineData("Chicago#$%", "#$%")]
    public void Constructor_WithInvalidCharacters_ShouldThrowArgumentException(string cityName, string invalidChars)
    {
        var exception = Assert.Throws<ArgumentException>(() => new City(cityName));
        exception.Message.Should().Contain($"City name contains invalid characters: '{invalidChars}'");
        exception.Message.Should().Contain("Only letters and certain special characters are allowed.");
    }

    [Fact]
    public void ToString_ShouldReturnCityValue()
    {
        const string cityName = "Seattle";
        var city = new City(cityName);

        string result = city.ToString();

        result.Should().Be(cityName);
    }

    [Fact]
    public void Equality_BetweenSameValues_ShouldBeTrue()
    {
        var city1 = new City("Portland");
        var city2 = new City("Portland");

        city1.Should().Be(city2);
    }

    [Fact]
    public void Equality_BetweenDifferentValues_ShouldBeFalse()
    {
        var city1 = new City("Portland");
        var city2 = new City("Seattle");

        city1.Should().NotBe(city2);
    }
}
