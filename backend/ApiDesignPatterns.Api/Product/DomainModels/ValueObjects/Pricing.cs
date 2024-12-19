namespace backend.Product.DomainModels.ValueObjects;

public record Pricing
{
    private decimal _basePrice;
    private decimal _discountPercentage;
    private decimal _taxRate;

    public decimal BasePrice
    {
        get => _basePrice;
        set
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
        set
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
        set
        {
            if (value is < 0 or > 100)
            {
                throw new ArgumentException("Tax rate must be between 0 and 100.");
            }

            _taxRate = value;
        }
    }
}
