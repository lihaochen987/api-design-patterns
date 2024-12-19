namespace backend.Product.DomainModels.ValueObjects;

public record Pricing
{
    private readonly decimal _basePrice;
    private readonly decimal _discountPercentage;
    private readonly decimal _taxRate;

    public decimal BasePrice
    {
        get => _basePrice;
        init
        {
            if (value <= 0)
            {
                throw new ArgumentException("Product price must be greater than zero.");
            }

            _basePrice = value;
        }
    }

    public decimal DiscountPercentage
    {
        get => _discountPercentage;
        init
        {
            if (value is < 0 or > 100)
            {
                throw new ArgumentException("Discount percentage must be between 0 and 100.");
            }

            _discountPercentage = value;
        }
    }

    public decimal TaxRate
    {
        get => _taxRate;
        init
        {
            if (value is < 0 or > 100)
            {
                throw new ArgumentException("Tax rate must be between 0 and 100.");
            }

            _taxRate = value;
        }
    }
}
