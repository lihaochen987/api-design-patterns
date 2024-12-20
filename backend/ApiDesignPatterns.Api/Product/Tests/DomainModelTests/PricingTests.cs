using backend.Product.DomainModels.ValueObjects;
using Shouldly;
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

        pricing.ShouldNotBeNull();
        pricing.BasePrice.ShouldBe(basePrice);
        pricing.DiscountPercentage.ShouldBe(discountPercentage);
        pricing.TaxRate.ShouldBe(taxRate);
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
        Should.Throw<ArgumentException>(() => new Pricing(basePrice, discountPercentage, taxRate));
    }

    [Fact]
    public void Constructor_ValidEdgeValues_ShouldCreatePricingObject()
    {
        const decimal basePrice = 0.01m;
        const decimal discountPercentage = 0m;
        const decimal taxRate = 0m;

        var pricing = new Pricing(basePrice, discountPercentage, taxRate);

        pricing.ShouldNotBeNull();
        pricing.BasePrice.ShouldBe(basePrice);
        pricing.DiscountPercentage.ShouldBe(discountPercentage);
        pricing.TaxRate.ShouldBe(taxRate);
    }

    [Theory]
    [InlineData(1, 0, 0)]
    [InlineData(1, 100, 0)]
    [InlineData(1, 0, 100)]
    public void Constructor_ValidBoundaryValues_ShouldCreatePricingObject(
        decimal basePrice,
        decimal discountPercentage,
        decimal taxRate)
    {
        var pricing = new Pricing(basePrice, discountPercentage, taxRate);

        pricing.ShouldNotBeNull();
        pricing.BasePrice.ShouldBe(basePrice);
        pricing.DiscountPercentage.ShouldBe(discountPercentage);
        pricing.TaxRate.ShouldBe(taxRate);
    }
}
