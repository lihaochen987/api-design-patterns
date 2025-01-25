namespace backend.Product.DomainModels.ValueObjects;

public record Pricing
{
    private Pricing()
    {
    }

    public decimal BasePrice { get; init; }

    public decimal DiscountPercentage { get; init; }

    public decimal TaxRate { get; init; }

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
