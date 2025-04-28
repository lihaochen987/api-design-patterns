namespace backend.Product.DomainModels.ValueObjects;

/// <summary>
/// Represents product pricing information including base price, discount percentage, and tax rate.
/// </summary>
public record Pricing
{
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
    /// Initializes a new instance of the Pricing record with validated price components.
    /// </summary>
    /// <param name="basePrice">The base price in currency units (must be greater than 0).</param>
    /// <param name="discountPercentage">The discount percentage (0-100).</param>
    /// <param name="taxRate">The tax rate percentage (0-100).</param>
    public Pricing(decimal basePrice, decimal discountPercentage, decimal taxRate)
    {
        if (!IsValid(basePrice, discountPercentage, taxRate))
        {
            throw new ArgumentException("Invalid value for Pricing");
        }

        BasePrice = basePrice;
        DiscountPercentage = discountPercentage;
        TaxRate = taxRate;
    }

    /// <summary>
    /// Validates the pricing components against business rules.
    /// </summary>
    /// <param name="basePrice">The base price to validate.</param>
    /// <param name="discountPercentage">The discount percentage to validate.</param>
    /// <param name="taxRate">The tax rate to validate.</param>
    /// <returns>True if all pricing components are within valid ranges; otherwise false.</returns>
    private static bool IsValid(decimal basePrice, decimal discountPercentage, decimal taxRate)
    {
        if (basePrice <= 0)
        {
            return false;
        }

        if (discountPercentage is < 0 or > 100)
        {
            return false;
        }

        return taxRate is >= 0 and <= 100;
    }
}
