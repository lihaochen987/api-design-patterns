namespace backend.Product.DomainModels;

public record DiscountPercentage
{
    private readonly decimal _value;

    private DiscountPercentage()
    {
    }

    public DiscountPercentage(decimal value)
    {
        if (value is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(value), "Discount percentage must be between 0 and 100.");

        _value = value;
    }

    public static bool IsValid(decimal candidate)
    {
        return candidate is >= 0 and <= 100;
    }

    public static bool TryParse(string candidate, out DiscountPercentage? discountPercentage)
    {
        discountPercentage = null;
        if (!decimal.TryParse(candidate, out var result))
            return false;

        if (!IsValid(result))
            return false;

        discountPercentage = new DiscountPercentage(result);
        return true;
    }

    public static implicit operator decimal(DiscountPercentage discountPercentage)
    {
        return discountPercentage._value;
    }
}