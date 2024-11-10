namespace backend.Product.DomainModels;

public record ProductPricing
{
    public long Id { get; set; }
    public decimal BasePrice { get; private set; }
    public DiscountPercentage DiscountPercentage { get; private set; }
    public TaxRate TaxRate { get; private set; }

    public void Replace(
        decimal basePrice,
        decimal discountPercentage,
        decimal taxRate)
    {
        EnforceInvariants(basePrice);
        BasePrice = basePrice;
        DiscountPercentage = new DiscountPercentage(discountPercentage);
        TaxRate = new TaxRate(taxRate);
    }

    private static void EnforceInvariants(decimal basePrice)
    {
        if (basePrice <= 0)
            throw new ArgumentException("Product price must be greater than zero.");
    }
}