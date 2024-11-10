namespace backend.Product.DomainModels;

public record ProductPricing
{
    public long Id { get; set; }
    public decimal BasePrice { get; private set; }
    public DiscountPercentage DiscountPercentage { get; private set; }
    public TaxRate TaxRate { get; private set; }

    public decimal Price
    {
        get => CalculatePrice();
        init => _ = value;
    }

    private decimal CalculatePrice()
    {
        var discountedPrice = BasePrice * (1 - (decimal)DiscountPercentage / 100);
        var finalPrice = discountedPrice * (1 + TaxRate / 100);
        return Math.Round(finalPrice, 2);
    }

    public void Replace(
        decimal basePrice,
        DiscountPercentage discountPercentage,
        TaxRate taxRate)
    {
        EnforceInvariants(basePrice);
        BasePrice = basePrice;
        DiscountPercentage = discountPercentage;
        TaxRate = taxRate;
    }

    private static void EnforceInvariants(decimal basePrice)
    {
        if (basePrice <= 0)
            throw new ArgumentException("Product price must be greater than zero.");
    }
}