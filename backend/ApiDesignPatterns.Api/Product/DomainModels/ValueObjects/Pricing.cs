using backend.Product.DomainModels.Exceptions;

namespace backend.Product.DomainModels.ValueObjects;

/// <summary>
/// Represents product pricing information including base price, discount percentage, and tax rate.
/// </summary>
public record Pricing
{
    private const decimal HighValueThreshold = 1000m;
    private const decimal MaxEffectiveDiscountPercent = 30m;
    private const decimal MinProfitMarginPercent = 15m;
    private const decimal EstimatedCostRatio = 0.6m;

    /// <summary>
    /// Private constructor for JSON deserialization and object mapping.
    /// </summary>
    private Pricing()
    {
    }

    /// <summary>
    /// Gets the base price of the product before discounts and taxes.
    /// </summary>
    /// <value>The base price in currency units.</value>
    public decimal BasePrice { get; init; }

    /// <summary>
    /// Gets the discount percentage to be applied to the base price.
    /// </summary>
    /// <value>The discount percentage.</value>
    public decimal DiscountPercentage { get; init; }

    /// <summary>
    /// Gets the tax rate to be applied after discounts.
    /// </summary>
    /// <value>The tax rate percentage.</value>
    public decimal TaxRate { get; init; }

    /// <summary>
    /// Calculates the final price after applying discount and tax.
    /// </summary>
    /// <returns>The final price after discounts and taxes.</returns>
    private decimal CalculateFinalPrice()
    {
        decimal discountAmount = BasePrice * (DiscountPercentage / 100m);
        decimal discountedPrice = BasePrice - discountAmount;
        decimal taxAmount = discountedPrice * (TaxRate / 100m);
        return discountedPrice + taxAmount;
    }

    /// <summary>
    /// Initializes a new instance of the Pricing record with validated price components.
    /// </summary>
    /// <param name="basePrice">The base price in currency units (must be greater than 0).</param>
    /// <param name="discountPercentage">The discount percentage (0-100).</param>
    /// <param name="taxRate">The tax rate percentage (0-100).</param>
    /// <exception cref="ArgumentException">Thrown when any individual value is invalid.</exception>
    /// <exception cref="ExcessiveDiscountException">Thrown when high-value items have excessive effective discount.</exception>
    /// <exception cref="InsufficientMarginException">Thrown when standard items have insufficient profit margin.</exception>
    public Pricing(decimal basePrice, decimal discountPercentage, decimal taxRate)
    {
        ValidateBasePrice(basePrice);
        ValidateDiscountPercentage(discountPercentage);
        ValidateTaxRate(taxRate);

        BasePrice = basePrice;
        DiscountPercentage = discountPercentage;
        TaxRate = taxRate;

        if (basePrice > HighValueThreshold)
        {
            ValidateHighValueItemDiscount();
        }
        else
        {
            ValidateStandardItemMargin();
        }
    }

    /// <summary>
    /// Validates the base price.
    /// </summary>
    /// <param name="basePrice">The base price to validate.</param>
    /// <exception cref="ArgumentException">Thrown when base price is invalid.</exception>
    private static void ValidateBasePrice(decimal basePrice)
    {
        if (basePrice <= 0)
        {
            throw new ArgumentException("Base price must be greater than zero.");
        }
    }

    /// <summary>
    /// Validates the discount percentage.
    /// </summary>
    /// <param name="discountPercentage">The discount percentage to validate.</param>
    /// <exception cref="ArgumentException">Thrown when discount percentage is invalid.</exception>
    private static void ValidateDiscountPercentage(decimal discountPercentage)
    {
        if (discountPercentage is < 0 or > 100)
        {
            throw new ArgumentException("Discount percentage must be between 0 and 100.");
        }
    }

    /// <summary>
    /// Validates the tax rate.
    /// </summary>
    /// <param name="taxRate">The tax rate to validate.</param>
    /// <exception cref="ArgumentException">Thrown when tax rate is invalid.</exception>
    private static void ValidateTaxRate(decimal taxRate)
    {
        if (taxRate is < 0 or > 100)
        {
            throw new ArgumentException("Tax rate must be between 0 and 100.");
        }
    }

    /// <summary>
    /// Validates that high-value items don't have excessive effective discount.
    /// </summary>
    /// <exception cref="ExcessiveDiscountException">Thrown when the effective discount exceeds the maximum allowed.</exception>
    private void ValidateHighValueItemDiscount()
    {
        decimal finalPrice = CalculateFinalPrice();
        decimal effectiveDiscount = (BasePrice - finalPrice) / BasePrice * 100m;

        if (effectiveDiscount > MaxEffectiveDiscountPercent)
        {
            throw new ExcessiveDiscountException(effectiveDiscount, MaxEffectiveDiscountPercent);
        }
    }

    /// <summary>
    /// Validates that standard items maintain a minimum profit margin.
    /// </summary>
    /// <exception cref="InsufficientMarginException">Thrown when the profit margin is below the minimum required.</exception>
    private void ValidateStandardItemMargin()
    {
        decimal estimatedCost = BasePrice * EstimatedCostRatio;
        decimal discountedPrice = BasePrice * (1 - DiscountPercentage / 100m);

        if (discountedPrice == 0)
        {
            throw new InsufficientMarginException(0, MinProfitMarginPercent);
        }

        decimal margin = (discountedPrice - estimatedCost) / discountedPrice * 100m;

        if (margin < MinProfitMarginPercent)
        {
            throw new InsufficientMarginException(margin, MinProfitMarginPercent);
        }
    }
}
