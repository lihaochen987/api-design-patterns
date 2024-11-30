namespace backend.Product.DomainModels.ValueObjects;

public record Pricing
{
    private Pricing()
    {
    }

    public Pricing(
        decimal basePrice,
        decimal discountPercentage,
        decimal taxRate)
    {
        EnforceInvariants(basePrice, discountPercentage, taxRate);
        BasePrice = basePrice;
        DiscountPercentage = discountPercentage;
        TaxRate = taxRate;
    }

    public decimal BasePrice { get; private set; }
    public decimal DiscountPercentage { get; private set; }
    public decimal TaxRate { get; private set; }

    private static void EnforceInvariants(
        decimal basePrice,
        decimal discountPercentage,
        decimal taxRate)
    {
        if (basePrice <= 0)
            throw new ArgumentException("Product price must be greater than zero.");
        if (discountPercentage is < 0 or > 100)
            throw new ArgumentException("Discount percentage must be between 0 and 100.");
        if (taxRate is < 0 or > 100)
            throw new ArgumentException("Tax rate must be between 0 and 100.");
    }
}