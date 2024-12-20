// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.ValueObjects;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.DomainModelTests;

public class DimensionsTests
{
    [Fact]
    public void Constructor_ValidDimensions_ShouldSetPropertiesCorrectly()
    {
        const decimal length = 50;
        const decimal width = 25;
        const decimal height = 20;

        var dimensions = new Dimensions(length, width, height);

        dimensions.Length.ShouldBeEquivalentTo(length);
        dimensions.Width.ShouldBeEquivalentTo(width);
        dimensions.Height.ShouldBeEquivalentTo(height);
    }

    [Fact]
    public void Constructor_InvalidLength_ShouldThrowArgumentException()
    {
        const decimal length = -1;
        const decimal width = 25;
        const decimal height = 20;

        Should.Throw<ArgumentException>(() => new Dimensions(length, width, height));
    }

    [Fact]
    public void Constructor_InvalidWidth_ShouldThrowArgumentException()
    {
        const decimal length = 50;
        const decimal width = -1;
        const decimal height = 20;

        Should.Throw<ArgumentException>(() => new Dimensions(length, width, height));
    }

    [Fact]
    public void Constructor_InvalidHeight_ShouldThrowArgumentException()
    {
        const decimal length = 50;
        const decimal width = 25;
        const decimal height = -1;

        Should.Throw<ArgumentException>(() => new Dimensions(length, width, height));
    }

    [Theory]
    [InlineData(101, 25, 20)]
    [InlineData(50, 51, 20)]
    [InlineData(50, 25, 51)]
    public void Constructor_OutOfRangeDimensions_ShouldThrowArgumentException(
        decimal length,
        decimal width,
        decimal height)
    {
        Should.Throw<ArgumentException>(() => new Dimensions(length, width, height));
    }

    [Fact]
    public void Constructor_VolumeExceedsMaximum_ShouldThrowArgumentException()
    {
        const decimal length = 100;
        const decimal width = 50;
        const decimal height = 50;

        Should.Throw<ArgumentException>(() => new Dimensions(length, width, height));
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(50, 25, 10)]
    public void Constructor_ValidDimensions_ShouldNotThrowException(decimal length, decimal width, decimal height)
    {
        Should.NotThrow(() => new Dimensions(length, width, height));
    }
}
