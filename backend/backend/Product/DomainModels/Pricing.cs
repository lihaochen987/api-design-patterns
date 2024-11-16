namespace backend.Product.DomainModels;

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
        EnforceInvariants(basePrice);
        BasePrice = basePrice;
        DiscountPercentage = new DiscountPercentage(discountPercentage);
        TaxRate = new TaxRate(taxRate);
    }

    public decimal BasePrice { get; private set; }
    public DiscountPercentage DiscountPercentage { get; private set; }
    public TaxRate TaxRate { get; private set; }

    private static void EnforceInvariants(decimal basePrice)
    {
        if (basePrice <= 0)
            throw new ArgumentException("Product price must be greater than zero.");
    }
}