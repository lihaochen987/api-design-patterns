using backend.Product.DomainModels.ValueObjects;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.DomainModelTests;

public class PricingTests
{
    [Fact]
    public void Constructor_ValidValues_ShouldCreatePricingObject()
    {
        const decimal basePrice = 100m;
        const decimal discountPercentage = 10m;
        const decimal taxRate = 15m;

        var pricing = new Pricing(basePrice, discountPercentage, taxRate);

        pricing.Should().NotBeNull();
        pricing.BasePrice.Should().Be(basePrice);
        pricing.DiscountPercentage.Should().Be(discountPercentage);
        pricing.TaxRate.Should().Be(taxRate);
    }

    [Theory]
    [InlineData(0, 10, 15)] // Invalid base price
    [InlineData(-100, 10, 15)] // Invalid base price
    [InlineData(100, -10, 15)] // Invalid discount percentage
    [InlineData(100, 110, 15)] // Invalid discount percentage
    [InlineData(100, 10, -5)] // Invalid tax rate
    [InlineData(100, 10, 150)] // Invalid tax rate
    public void Constructor_InvalidValues_ShouldThrowArgumentException(decimal basePrice,
        decimal discountPercentage, decimal taxRate)
    {
        Action act = () => _ = new Pricing(basePrice, discountPercentage, taxRate);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_ValidEdgeValues_ShouldCreatePricingObject()
    {
        const decimal basePrice = 0.01m;
        const decimal discountPercentage = 0m;
        const decimal taxRate = 0m;

        var pricing = new Pricing(basePrice, discountPercentage, taxRate);

        pricing.Should().NotBeNull();
        pricing.BasePrice.Should().Be(basePrice);
        pricing.DiscountPercentage.Should().Be(discountPercentage);
        pricing.TaxRate.Should().Be(taxRate);
    }

    [Theory]
    [InlineData(10, 0, 0)]      // Zero discount, zero tax
    [InlineData(10, 20, 0)]     // Moderate discount, zero tax
    [InlineData(10, 0, 20)]     // Zero discount, with tax
    [InlineData(100, 15, 10)]   // Standard case
    [InlineData(2000, 10, 10)]  // High-value item
    public void Constructor_ValidBoundaryValues_ShouldCreatePricingObject(
        decimal basePrice,
        decimal discountPercentage,
        decimal taxRate)
    {
        var pricing = new Pricing(basePrice, discountPercentage, taxRate);

        pricing.Should().NotBeNull();
        pricing.BasePrice.Should().Be(basePrice);
        pricing.DiscountPercentage.Should().Be(discountPercentage);
        pricing.TaxRate.Should().Be(taxRate);
    }

    [Fact]
    public void Constructor_HighValueItemWithExcessiveDiscounting_ShouldThrowArgumentException()
    {
        const decimal basePrice = 2000m;
        const decimal discountPercentage = 40m;
        const decimal taxRate = 5m;

        Action act = () => _ = new Pricing(basePrice, discountPercentage, taxRate);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Excessive effective discount: 37.00%. Maximum allowed: 30.00%");
    }

    [Fact]
    public void Constructor_LowPriceItemWithInsufficientMargin_ShouldThrowArgumentException()
    {
        const decimal basePrice = 500m;
        const decimal discountPercentage = 30m;
        const decimal taxRate = 10m;

        Action act = () => _ = new Pricing(basePrice, discountPercentage, taxRate);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Insufficient profit margin: 14.29%. Minimum required: 15.00%");
    }
}
