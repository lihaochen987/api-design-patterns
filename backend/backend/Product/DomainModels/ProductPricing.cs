namespace backend.Product.DomainModels;

public record ProductPricing
{
    private ProductPricing()
    {
    }

    public ProductPricing(
        long id,
        decimal basePrice,
        decimal discountPercentage,
        decimal taxRate)
    {
        EnforceInvariants(basePrice);
        Id = id;
        BasePrice = basePrice;
        DiscountPercentage = new DiscountPercentage(discountPercentage);
        TaxRate = new TaxRate(taxRate);
    }

    public ProductPricing(
        decimal basePrice,
        decimal discountPercentage,
        decimal taxRate)
    {
        EnforceInvariants(basePrice);
        BasePrice = basePrice;
        DiscountPercentage = new DiscountPercentage(discountPercentage);
        TaxRate = new TaxRate(taxRate);
    }

    public long Id { get; private set; }
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